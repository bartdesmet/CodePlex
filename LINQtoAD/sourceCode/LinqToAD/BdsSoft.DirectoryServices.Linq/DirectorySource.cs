/*
 * LINQ to Active Directory
 * http://www.codeplex.com/LINQtoAD
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoAD/Project/License.aspx for more information.
 */

#region Namespace imports

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

#endregion

namespace BdsSoft.DirectoryServices.Linq
{
    /// <summary>
    /// Represents a source object used to tak to Directory Services.
    /// </summary>
    public interface IDirectorySource
    {
        #region Properties

        /// <summary>
        /// Gets the logger for the source. Will be used in logging query information.
        /// </summary>
        TextWriter Log { get; }

        /// <summary>
        /// Root location captured by this source.
        /// </summary>
        DirectoryEntry Root { get; }

        /// <summary>
        /// Search scope used when running queries against this source.
        /// </summary>
        SearchScope Scope { get; }

        /// <summary>
        /// Entity type this source will query for.
        /// </summary>
        Type OriginalType { get; }

        /// <summary>
        /// Searcher used to perform directory searches.
        /// </summary>
        DirectorySearcher Searcher { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Callback method for entity property change tracking.
        /// </summary>
        void UpdateNotification(object sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Update changes in the underlying data source.
        /// </summary>
        void Update();

        #endregion
    }

    /// <summary>
    /// Represents an LDAP data source. Allows for querying the LDAP data source via LINQ.
    /// </summary>
    /// <typeparam name="T">Entity type in the underlying source.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class DirectorySource<T> : IQueryable<T>, IDirectorySource
    {
        #region Private members

        /// <summary>
        /// Associated directory context if bound to a context; otherwise null.
        /// </summary>
        private DirectoryContext _context;

        /// <summary>
        /// Searcher object to perform the directory search. Captures the search root and options.
        /// </summary>
        private DirectorySearcher _searcher;

        /// <summary>
        /// Logger for diagnostic output.
        /// </summary>
        private TextWriter logger;

        /// <summary>
        /// Update catalog; keeps track of update entity instances.
        /// </summary>
        private Dictionary<object, HashSet<string>> updates = new Dictionary<object, HashSet<string>>();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new data source instance for the given directory search root and with a given search scope.
        /// </summary>
        /// <param name="searchRoot">Root location in the directory to start all searches from.</param>
        /// <param name="searchScope">Search scope for all queries performed through this data source.</param>
        public DirectorySource(DirectoryEntry searchRoot, SearchScope searchScope)
        {
            _searcher = new DirectorySearcher(searchRoot) { SearchScope = searchScope };
        }

        /// <summary>
        /// Creates a new data source instance based on the given DirectorySearcher.
        /// </summary>
        /// <param name="searcher">DirectorySearcher object to use for directory searches.</param>
        public DirectorySource(DirectorySearcher searcher)
        {
            _searcher = searcher;
        }

        /// <summary>
        /// Creates a data source from the given context.
        /// </summary>
        /// <param name="context">DirectoryContext that embeds the data source.</param>
        /// <param name="searchScope">Search scope for all queries performed through this data source.</param>
        internal DirectorySource(DirectoryContext context, SearchScope searchScope)
        {
            _context = context;
            _searcher = Helpers.CloneSearcher(context.Searcher, null, null);
            _searcher.SearchScope = searchScope;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Used to configure a logger to print diagnostic information about the query performed.
        /// </summary>
        public TextWriter Log
        {
            get
            {
                if (logger != null)
                    return logger;

                // Fall back to parent context if applicable.
                if (_context != null)
                    return _context.Log;

                return null;
            }
            set
            {
                // Can override on the leaf level.
                logger = value;
            }
        }

        /// <summary>
        /// Search root.
        /// </summary>
        public DirectoryEntry Root
        {
            get { return Searcher.SearchRoot; }
        }

        /// <summary>
        /// Search scope.
        /// </summary>
        public SearchScope Scope
        {
            get { return Searcher.SearchScope; }
        }

        /// <summary>
        /// Entity element type.
        /// </summary>
        public Type ElementType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// Original type of objects being queried.
        /// </summary>
        public Type OriginalType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// Expression representing the data source object.
        /// </summary>
        public Expression Expression
        {
            get { return Expression.Constant(this); }
        }

        /// <summary>
        /// LINQ query provider object.
        /// </summary>
        public IQueryProvider Provider
        {
            get { return new DirectoryQueryProvider(); }
        }

        /// <summary>
        /// Directory searcher used for the directory search.
        /// </summary>
        public DirectorySearcher Searcher
        {
            get { return _searcher; }
        }

        #endregion

        #region Enumeration

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DirectoryQuery<T>(this.Expression).GetEnumerator();
        }

        #endregion

        #region Updates

        public void Update()
        {
            Type t = typeof(T);
            DirectorySchemaAttribute[] attr = (DirectorySchemaAttribute[])t.GetCustomAttributes(typeof(DirectorySchemaAttribute), false);

            foreach (var e in updates)
            {
                if (e.Key is T && e.Key is DirectoryEntity)
                {
                    DirectoryEntry entry = ((DirectoryEntity)e.Key).DirectoryEntry;
                    foreach (string property in e.Value)
                    {
                        PropertyInfo i = t.GetProperty(property);

                        DirectoryAttributeAttribute[] da = i.GetCustomAttributes(typeof(DirectoryAttributeAttribute), false) as DirectoryAttributeAttribute[];
                        if (da != null && da.Length != 0 && da[0] != null)
                        {
                            if (da[0].QuerySource == DirectoryAttributeType.ActiveDs)
                            {
                                if (attr != null && attr.Length != 0)
                                    attr[0].ActiveDsHelperType.GetProperty(da[0].Attribute).SetValue(entry.NativeObject, i.GetValue(e.Key, null), null);
                                else
                                    throw new InvalidOperationException("Missing schema mapping attribute for updates through ADSI.");
                            }
                            else
                                entry.Properties[da[0].Attribute].Value = i.GetValue(e.Key, null);
                        }
                        else
                            entry.Properties[i.Name].Value = i.GetValue(e.Key, null);
                    }
                    entry.CommitChanges();
                }
                else
                    throw new InvalidOperationException("Can't apply update because updates type doesn't match original entity type.");
            }

            updates.Clear();
        }

        public void UpdateNotification(object sender, PropertyChangedEventArgs e)
        {
            T source = (T)sender;

            if (!updates.ContainsKey(source))
                updates.Add(source, new HashSet<string>());

            updates[source].Add(e.PropertyName);
        }

        #endregion
    }

    /// <summary>
    /// LINQ query provider for Directory Services.
    /// </summary>
    public class DirectoryQueryProvider : IQueryProvider
    {
        #region CreateQuery implementation

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs an IQueryable object that can evaluate the query represented by the specified expression tree. 
        /// </summary>
        /// <param name="expression">Expression representing the LDAP query.</param>
        /// <typeparam name="TElement">The type of the elements of the IQueryable that is returned.</typeparam>
        /// <returns>IQueryable object that can evaluate the query represented by the specified expression tree.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new DirectoryQuery<TElement>(expression);
        }

        #endregion

        #region Direct execution (not implemented)

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public TResult Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Represents a query in Directory Services.
    /// </summary>
    /// <typeparam name="T">Type of the objects returned by this query upon execution.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class DirectoryQuery<T> : IQueryable<T>
    {
        #region Private members

        private Expression _ex;

        private IDirectorySource _source;

        private Type originalType;

        #region Projection information

        private HashSet<string> properties = new HashSet<string>();
        private Delegate project;

        #endregion

        #region Query information

        private string query;

        #endregion

        #endregion

        #region Constructors

        internal DirectoryQuery(Expression ex)
        {
            _ex = ex;
        }

        #endregion

        #region Properties

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get { return _ex; }
        }

        public IQueryProvider Provider
        {
            get { return new DirectoryQueryProvider(); }
        }

        #endregion

        #region Methods

        #region Query execution

        /// <summary>
        /// Executes the query and returns results.
        /// </summary>
        /// <returns>Query results represented as entity objects or projection results.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            Parse();
            return GetResults();
        }

        /// <summary>
        /// Executes the query and returns results.
        /// </summary>
        /// <returns>Query results represented as entity objects or projection results.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Executes the query and returns results.
        /// </summary>
        /// <returns>Query results represented as entity objects or projection results.</returns>
        private IEnumerator<T> GetResults()
        {
            DirectorySchemaAttribute[] attr = (DirectorySchemaAttribute[])originalType.GetCustomAttributes(typeof(DirectorySchemaAttribute), false);
            if (attr == null || attr.Length == 0)
                throw new InvalidOperationException("Missing schema mapping attribute.");

            string classQuery = String.Format("(objectClass={0})", attr[0].Schema);

            DirectorySearcher s = Helpers.CloneSearcher(
                _source.Searcher,
                !string.IsNullOrEmpty(query) ? String.Format("(&{0}{1})", classQuery, query) : classQuery,
                properties.ToArray()
            );

            if (_source.Log != null)
                _source.Log.WriteLine(s.Filter);

            Type helper = attr[0].ActiveDsHelperType;

            foreach (SearchResult sr in s.FindAll())
            {
                DirectoryEntry e = sr.GetDirectoryEntry();

                object result = Activator.CreateInstance(project == null ? typeof(T) : originalType);

                /// *** UPDATE ***
                DirectoryEntity entity = result as DirectoryEntity;
                if (entity != null)
                    entity.DirectoryEntry = e;

                if (project == null)
                {
                    foreach (PropertyInfo p in typeof(T).GetProperties())
                        AssignResultProperty(helper, sr, result, p.Name);

                    /// *** UPDATE ***
                    if (entity != null)
                        entity.PropertyChanged += new PropertyChangedEventHandler(_source.UpdateNotification);

                    yield return (T)result;
                }
                else
                {
                    foreach (string prop in properties)
                        AssignResultProperty(helper, sr, result, prop);

                    /// *** UPDATE ***
                    if (entity != null)
                        entity.PropertyChanged += new PropertyChangedEventHandler(_source.UpdateNotification);

                    yield return (T)project.DynamicInvoke(result);
                }
            }
        }

        /// <summary>
        /// Assigns the specified property on the specified object based on the data wrapped in the DirectoryEntry representing the current result.
        /// </summary>
        /// <param name="helper">Active Directory helper type to help retrieving the target value.</param>
        /// <param name="searchResult">SearchResult object containing the data for the current result.</param>
        /// <param name="result">Object the property has to be set on.</param>
        /// <param name="prop">Property to be set.</param>
        private void AssignResultProperty(Type helper, SearchResult searchResult, object result, string prop)
        {
            PropertyInfo i = originalType.GetProperty(prop);
            DirectoryAttributeAttribute[] da = i.GetCustomAttributes(typeof(DirectoryAttributeAttribute), false) as DirectoryAttributeAttribute[];
            if (da != null && da.Length != 0)
            {
                if (da[0].QuerySource == DirectoryAttributeType.ActiveDs)
                {
                    PropertyInfo p = helper.GetProperty(da[0].Attribute, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    try
                    {
                        i.SetValue(result, p.GetValue(searchResult.GetDirectoryEntry().NativeObject, null), null);
                    }
                    catch (TargetInvocationException) { }
                }
                else
                {
                    DirectoryEntry e = searchResult.GetDirectoryEntry();

                    var resultValue = e.Properties[da[0].Attribute];
                    if (i.PropertyType.IsArray)
                    {
                        //
                        // Byte array properties are special in AD. Here we don't follow the
                        // heuristic of an array-typed property to be an expanded multi-value
                        // property but we support splatting the contents of the value into
                        // the byte[] array.
                        //
                        byte[] value = null;
                        if (i.PropertyType.GetElementType() == typeof(byte) && (value = resultValue[0] as byte[]) != null)
                        {
                            i.SetValue(result, value, null);
                        }
                        else
                        {
                            Array o = Array.CreateInstance(i.PropertyType.GetElementType(), resultValue.Count);

                            int j = 0;
                            foreach (object oo in resultValue)
                                o.SetValue(oo, j++);

                            i.SetValue(result, o, null);
                        }
                    }
                    else
                    {
                        if (resultValue.Count == 1)
                        {
                            //
                            // Support GUID field mapping.
                            //
                            if (i.PropertyType == typeof(Guid))
                            {
                                byte[] value = resultValue[0] as byte[];
                                if (value == null)
                                    throw new NotSupportedException("Mapping of Guid-typed property " + i.Name + " to non-byte[] valued directory field " + da[0].Attribute + ".");

                                i.SetValue(result, new Guid(value), null);
                            }
                            else
                                i.SetValue(result, resultValue[0], null);
                        }
                    }
                }
            }
            else
            {
                var pvc = searchResult.GetDirectoryEntry().Properties[prop];
                if (pvc.Count == 1)
                    i.SetValue(result, pvc[0], null);
            }
        }

        #endregion

        #region Query parsing

        /// <summary>
        /// Parses the query expression tree captured by this query instance.
        /// </summary>
        public void Parse()
        {
            Parse(_ex);
        }

        /// <summary>
        /// Parses the specified query expression tree.
        /// </summary>
        /// <param name="ex"></param>
        public void Parse(Expression ex)
        {
            var ce = ex as ConstantExpression;
            var mce = ex as MethodCallExpression;

            if (ce != null)
            {
                _source = ce.Value as IDirectorySource;
                originalType = _source.OriginalType;
            }
            else if (mce != null)
            {
                //
                // Should be extension methods on Queryable.
                //
                if (mce.Method.DeclaringType != typeof(Queryable))
                    throw new NotSupportedException("Detected invalid top-level method-call.");

                Parse(mce.Arguments[0]);

                //
                // First parameter to the method call represents the (unary) lambda in LINQ style.
                // E.g. (user => user.Name == "Bart") for a Where  clause
                //      (user => new { user.Name })   for a Select clause
                //
                switch (mce.Method.Name)
                {
                    //
                    // Builds the query LDAP expression.
                    //
                    case "Where":
                        BuildPredicate(((UnaryExpression)mce.Arguments[1]).Operand as LambdaExpression);
                        break;
                    //
                    // Builds the projection and filters the required properties.
                    //
                    case "Select":
                        BuildProjection(((UnaryExpression)mce.Arguments[1]).Operand as LambdaExpression);
                        break;
                    default:
                        throw new NotSupportedException("Unsupported query operator: " + mce.Method.Name);
                }
            }
            else
                throw new NotSupportedException("Invalid expression node detected.");
        }

        #region Projection support

        /// <summary>
        /// Helper method for projection clauses (Select).
        /// </summary>
        /// <param name="p">Lambda expression representing the projection.</param>
        private void BuildProjection(LambdaExpression p)
        {
            //
            // Store projection information including the compiled lambda for subsequent execution
            // and a minimal set of properties to be retrieved (improves efficiency of queries).
            //
            project = p.Compile();

            //
            // Original type is kept for reflection during querying.
            //
            originalType = p.Parameters[0].Type;

            //
            // Support for (anonymous) type initialization based on "member init expressions".
            //
            MemberInitExpression mi = p.Body as MemberInitExpression;
            if (mi != null)
            {
                Queue<MemberBinding> bindings = new Queue<MemberBinding>();
                foreach (MemberBinding b in mi.Bindings)
                    bindings.Enqueue(b);

                while (bindings.Count > 0)
                {
                    MemberBinding b = bindings.Dequeue();

                    MemberAssignment ma;
                    MemberMemberBinding mmb;
                    MemberListBinding mlb;

                    if ((ma = b as MemberAssignment) != null)
                        FindProperties(ma.Expression);
                    else if ((mmb = b as MemberMemberBinding) != null)
                        foreach (MemberBinding mb in mmb.Bindings)
                            bindings.Enqueue(mb);
                    else if ((mlb = b as MemberListBinding) != null)
                        foreach (ElementInit ie in mlb.Initializers)
                            foreach (Expression arg in ie.Arguments)
                                FindProperties(arg);
                }
            }
            //
            // Support for identity projections (e.g. user => user), getting all properties back.
            //
            else
                foreach (PropertyInfo i in originalType.GetProperties())
                    properties.Add(i.Name);
        }

        /// <summary>
        /// Recursive helper method to finds all required properties for projection.
        /// </summary>
        /// <param name="e">Expression to detect property uses for.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private void FindProperties(Expression e)
        {
            //
            // Record member accesses to properties or fields from the entity.
            //
            if (e.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression me = e as MemberExpression;
                if (me.Member.DeclaringType == originalType)
                {
                    DirectoryAttributeAttribute[] da = me.Member.GetCustomAttributes(typeof(DirectoryAttributeAttribute), false) as DirectoryAttributeAttribute[];
                    if (da != null && da.Length != 0)
                    {
                        if (da[0].QuerySource != DirectoryAttributeType.ActiveDs)
                            properties.Add(me.Member.Name);
                    }
                    else
                        properties.Add(me.Member.Name);
                }
            }
            else
            {
                BinaryExpression b;
                UnaryExpression u;
                ConditionalExpression c;
                InvocationExpression i;
                LambdaExpression l;
                ListInitExpression li;
                MemberInitExpression mi;
                MethodCallExpression mc;
                NewExpression n;
                NewArrayExpression na;
                TypeBinaryExpression tb;

                if ((b = e as BinaryExpression) != null)
                {
                    FindProperties(b.Left);
                    FindProperties(b.Right);
                }
                else if ((u = e as UnaryExpression) != null)
                {
                    FindProperties(u.Operand);
                }
                else if ((c = e as ConditionalExpression) != null)
                {
                    FindProperties(c.IfFalse);
                    FindProperties(c.IfTrue);
                    FindProperties(c.Test);
                }
                else if ((i = e as InvocationExpression) != null)
                {
                    FindProperties(i.Expression);
                    foreach (Expression ex in i.Arguments)
                        FindProperties(ex);
                }
                else if ((l = e as LambdaExpression) != null)
                {
                    FindProperties(l.Body);
                    foreach (Expression ex in l.Parameters)
                        FindProperties(ex);
                }
                else if ((li = e as ListInitExpression) != null)
                {
                    FindProperties(li.NewExpression);
                    foreach (ElementInit init in li.Initializers)
                        foreach (Expression ex in init.Arguments)
                            FindProperties(ex);
                }
                else if ((mi = e as MemberInitExpression) != null)
                {
                    FindProperties(mi.NewExpression);
                    foreach (MemberAssignment ma in mi.Bindings)
                        FindProperties(ma.Expression);
                }
                else if ((mc = e as MethodCallExpression) != null)
                {
                    FindProperties(mc.Object);
                    foreach (Expression ex in mc.Arguments)
                        FindProperties(ex);
                }
                else if ((n = e as NewExpression) != null)
                {
                    foreach (Expression ex in n.Arguments)
                        FindProperties(ex);
                }
                else if ((na = e as NewArrayExpression) != null)
                {
                    foreach (Expression ex in na.Expressions)
                        FindProperties(ex);
                }
                else if ((tb = e as TypeBinaryExpression) != null)
                {
                    FindProperties(tb.Expression);
                }
            }
        }

        #endregion

        #region Query predicate support

        /// <summary>
        /// Helper method to build the LDAP query.
        /// </summary>
        /// <param name="q">Lambda expression to be translated to LDAP.</param>
        private void BuildPredicate(LambdaExpression q)
        {
            StringBuilder sb = new StringBuilder();

            //
            // Recursive tree traversal to build the LDAP query (prefix notation).
            //
            ParsePredicate(q.Body, sb);

            query = sb.ToString();
        }

        /// <summary>
        /// Recursive helper method for query parsing based on the given expression tree.
        /// </summary>
        /// <param name="e">Expression tree to be translated to LDAP.</param>
        /// <param name="sb">Accummulative query string used in recursion.</param>
        private void ParsePredicate(Expression e, StringBuilder sb)
        {
            BinaryExpression b;
            UnaryExpression u;
            MethodCallExpression m;

            sb.Append("(");
            //
            // Support for boolean operators & and |. Support for "raw" conditions (like equality).
            //
            if ((b = e as BinaryExpression) != null)
            {
                switch (b.NodeType)
                {
                    case ExpressionType.AndAlso:
                        sb.Append("&");
                        ParsePredicate(b.Left, sb);
                        ParsePredicate(b.Right, sb);
                        break;
                    case ExpressionType.OrElse:
                        sb.Append("|");
                        ParsePredicate(b.Left, sb);
                        ParsePredicate(b.Right, sb);
                        break;
                    default: //E.g. Equal, NotEqual, GreaterThan
                        sb.Append(GetCondition(b));
                        break;
                }
            }
            //
            // Support for boolean negation.
            //
            else if ((u = e as UnaryExpression) != null)
            {
                if (u.NodeType == ExpressionType.Not)
                {
                    sb.Append("!");
                    ParsePredicate(u.Operand, sb);
                }
                else
                    throw new NotSupportedException("Unsupported query operator detected: " + u.NodeType);
            }
            //
            // Support for string operations.
            //
            else if ((m = e as MethodCallExpression) != null)
            {
                MemberExpression o = (m.Object as MemberExpression);
                if (m.Method.DeclaringType == typeof(string))
                {
                    switch (m.Method.Name)
                    {
                        case "Contains":
                            {
                                string value = GetStringOperandValue(m);
                                sb.AppendFormat("{0}=*{1}*", GetFieldName(o.Member), value);
                                break;
                            }
                        case "StartsWith":
                            {
                                string value = GetStringOperandValue(m);
                                sb.AppendFormat("{0}={1}*", GetFieldName(o.Member), value);
                                break;
                            }
                        case "EndsWith":
                            {
                                string value = GetStringOperandValue(m);
                                sb.AppendFormat("{0}=*{1}", GetFieldName(o.Member), value);
                                break;
                            }
                        default:
                            throw new NotSupportedException("Unsupported string filtering query expression detected. Cannot translate to LDAP equivalent.");
                    }
                }
                else
                    throw new NotSupportedException("Unsupported query expression detected. Cannot translate to LDAP equivalent.");
            }
            else
                throw new NotSupportedException("Unsupported query expression detected. Cannot translate to LDAP equivalent.");
            sb.Append(")");
        }

        /// <summary>
        /// Helper method to get the first string-valued parameter of a method call expression.
        /// </summary>
        /// <param name="m">Method call expression to get the string-valued parameter from.</param>
        /// <returns>First parameter string value.</returns>
        private static string GetStringOperandValue(MethodCallExpression m)
        {
            Debug.Assert(m.Arguments.Count == 1);

            string value = Expression.Lambda(m.Arguments[0]).Compile().DynamicInvoke() as string;
            if (value == null)
                throw new NotSupportedException(m.Method.Name + " can only be used with string operands.");

            return value;
        }

        /// <summary>
        /// Helper method to translate conditions to LDAP filters.
        /// </summary>
        /// <param name="e">Conditional expression to be translated to an LDAP filter.</param>
        /// <returns>String representing the condition in LDAP.</returns>
        private string GetCondition(BinaryExpression e)
        {
            string val, attrib;

            bool neg;

            //
            // Find the order of the operands in the binary expression. At least one should refer to the entity type.
            //
            if (e.Left is MemberExpression && ((MemberExpression)e.Left).Member.DeclaringType == originalType)
            {
                neg = false;

                attrib = GetFieldName(((MemberExpression)e.Left).Member);
                val = Expression.Lambda(e.Right).Compile().DynamicInvoke().ToString();
            }
            else if (e.Right is MemberExpression && ((MemberExpression)e.Right).Member.DeclaringType == originalType)
            {
                neg = true;

                attrib = GetFieldName(((MemberExpression)e.Right).Member);
                val = Expression.Lambda(e.Left).Compile().DynamicInvoke().ToString();
            }
            else
                throw new NotSupportedException("A filtering expression should contain an entity member selection expression.");

            //
            // Normalize some common characters that cannot be used in LDAP filters.
            //
            val = val.ToString().Replace("(", "0x28").Replace(")", "0x29").Replace(@"\", "0x5c");

            //
            // Determine the operator and swap the operandi if necessary (LDAP requires a field=value order).
            //
            switch (e.NodeType)
            {
                case ExpressionType.Equal:
                    return String.Format(CultureInfo.InvariantCulture, "{0}={1}", attrib, val);
                case ExpressionType.NotEqual:
                    return String.Format(CultureInfo.InvariantCulture, "!({0}={1})", attrib, val);
                case ExpressionType.GreaterThanOrEqual:
                    if (!neg)
                        return String.Format(CultureInfo.InvariantCulture, "{0}>={1}", attrib, val);
                    else
                        return String.Format(CultureInfo.InvariantCulture, "{0}<={1}", attrib, val);
                case ExpressionType.GreaterThan:
                    if (!neg)
                        return String.Format(CultureInfo.InvariantCulture, "&({0}>={1})(!({0}={1}))", attrib, val);
                    else
                        return String.Format(CultureInfo.InvariantCulture, "&({0}<={1})(!({0}={1}))", attrib, val);
                case ExpressionType.LessThanOrEqual:
                    if (!neg)
                        return String.Format(CultureInfo.InvariantCulture, "{0}<={1}", attrib, val);
                    else
                        return String.Format(CultureInfo.InvariantCulture, "{0}>={1}", attrib, val);
                case ExpressionType.LessThan:
                    if (!neg)
                        return String.Format(CultureInfo.InvariantCulture, "&({0}<={1})(!({0}={1}))", attrib, val);
                    else
                        return String.Format(CultureInfo.InvariantCulture, "&({0}>={1})(!({0}={1}))", attrib, val);
                default:
                    throw new NotSupportedException("Unsupported filtering operator detected: " + e.NodeType);
            }
        }

        /// <summary>
        /// Gets the mapped field name for the specified entity type member.
        /// </summary>
        /// <param name="member">Entity type member.</param>
        /// <returns>Mapped field name. If no mapping is applied to the specified member, the member's name will be returned (default mapping).</returns>
        private static string GetFieldName(MemberInfo member)
        {
            DirectoryAttributeAttribute[] da = member.GetCustomAttributes(typeof(DirectoryAttributeAttribute), false) as DirectoryAttributeAttribute[];
            if (da != null && da.Length != 0 && da[0] != null)
            {
                if (da[0].QuerySource == DirectoryAttributeType.ActiveDs)
                    throw new InvalidOperationException("Can't execute query filters for ADSI properties.");
                else
                    return da[0].Attribute;
            }
            else
                return member.Name;
        }

        #endregion

        #endregion

        #endregion
    }

    /// <summary>
    /// Extension methods for System.DirectoryServices classes.
    /// </summary>
    public static class DirectoryExtensions
    {
        /// <summary>
        /// Lifts a DirectorySearcher object into a queryable DirectorySource for a given entity type.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="searcher">DirectorySearcher to base the query on.</param>
        /// <returns>Queryable DirectorySource.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static DirectorySource<T> AsQueryable<T>(this DirectorySearcher searcher)
        {
            return new DirectorySource<T>(searcher);
        }

        /// <summary>
        /// Lifts a DirectoryEntry object into a queryable DirectorySource for a given entity type.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="entry">DirectoryEntry to start the query from.</param>
        /// <param name="scope">Scope for the query.</param>
        /// <returns>Queryable DirectorySource.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static DirectorySource<T> AsQueryable<T>(this DirectoryEntry entry, SearchScope scope)
        {
            return new DirectorySource<T>(entry, scope);
        }
    }

    /// <summary>
    /// Provides context-based provider support.
    /// </summary>
    public class DirectoryContext : IDisposable
    {
        #region Private members

        private DirectorySearcher _searcher;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new data source instance for the given directory search root and with a given search scope.
        /// </summary>
        /// <param name="searchRoot">Root location in the directory to start all searches from.</param>
        public DirectoryContext(DirectoryEntry searchRoot)
        {
            _searcher = new DirectorySearcher(searchRoot);
            InitializeSources();
        }

        /// <summary>
        /// Creates a new data source instance based on the given DirectorySearcher.
        /// </summary>
        /// <param name="searcher">DirectorySearcher object to use for directory searches.</param>
        public DirectoryContext(DirectorySearcher searcher)
        {
            _searcher = searcher;
            InitializeSources();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Used to configure a logger to print diagnostic information about the query performed.
        /// </summary>
        public TextWriter Log { get; set; }

        /// <summary>
        /// Searcher object.
        /// </summary>
        public DirectorySearcher Searcher { get { return _searcher; } }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the DirectorySource for the given entity type.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="scope">Search scope.</param>
        /// <returns>DirectorySource for the given entity type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public DirectorySource<T> GetSource<T>(SearchScope scope)
        {
            return new DirectorySource<T>(this, scope);
        }

        /// <summary>
        /// Updates all data sources captured by the context object recursively (including nested contexts).
        /// </summary>
        public void Update()
        {
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                if (typeof(IDirectorySource).IsAssignableFrom(property.PropertyType))
                {
                    ((IDirectorySource)property.GetValue(this, null)).Update();
                }
                else if (typeof(DirectoryContext).IsAssignableFrom(property.PropertyType))
                {
                    ((DirectoryContext)property.GetValue(this, null)).Update();
                }
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Helper method to initialize data sources.
        /// </summary>
        private void InitializeSources()
        {
            MethodInfo getSource = this.GetType().GetMethod("GetSource");

            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(DirectorySource<>))
                {
                    var searchOptions = property.GetCustomAttributes(typeof(DirectorySearchOptionsAttribute), true).Cast<DirectorySearchOptionsAttribute>().FirstOrDefault();
                    SearchScope scope = searchOptions != null ? searchOptions.Scope : SearchScope.Base;

                    Type entity = property.PropertyType.GetGenericArguments()[0];
                    property.SetValue(this, getSource.MakeGenericMethod(entity).Invoke(this, new object[] { scope }), null);
                }
                else if (typeof(DirectoryContext).IsAssignableFrom(property.PropertyType))
                {
                    var searchPath = property.GetCustomAttributes(typeof(DirectorySearchPathAttribute), true).Cast<DirectorySearchPathAttribute>().FirstOrDefault();

                    DirectoryEntry searchRoot = _searcher.SearchRoot;
                    if (searchPath != null && !string.IsNullOrEmpty(searchPath.Path))
                    {
                        try
                        {
                            searchRoot = _searcher.SearchRoot.Children.Find(searchPath.Path);
                        }
                        catch (DirectoryServicesCOMException ex)
                        {
                            throw new InvalidOperationException("Failed to retrieve nested context " + property.Name + " with search path " + searchPath.Path + ".", ex);
                        }
                    }

                    Type subContext = property.PropertyType;
                    ConstructorInfo ctor = subContext.GetConstructor(new Type[] { typeof(DirectoryEntry) });
                    if (ctor == null)
                        throw new InvalidOperationException("Nested context " + property.Name + " does not have a suitable constructor.");

                    property.SetValue(this, ctor.Invoke(new object[] { searchRoot }), null);
                }
            }
        }

        #endregion

        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_searcher != null)
                {
                    _searcher.Dispose();
                    _searcher = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    internal static class Helpers
    {
        /// <summary>
        /// Helper method to clone a DirectorySearcher object and apply the specified filter and properties.
        /// </summary>
        /// <param name="searcher">DirectorySearcher object to clone.</param>
        /// <param name="filter">Search filter.</param>
        /// <param name="properties">Properties to load.</param>
        /// <returns>Cloned DirectorySearcher object with applied filter and properties.</returns>
        public static DirectorySearcher CloneSearcher(DirectorySearcher searcher, string filter, string[] properties)
        {
            DirectorySearcher result = new DirectorySearcher()
            {
                Asynchronous = searcher.Asynchronous,
                AttributeScopeQuery = searcher.AttributeScopeQuery,
                CacheResults = searcher.CacheResults,
                ClientTimeout = searcher.ClientTimeout,
                DerefAlias = searcher.DerefAlias,
                DirectorySynchronization = searcher.DirectorySynchronization,
                ExtendedDN = searcher.ExtendedDN,
                PageSize = searcher.PageSize,
                PropertyNamesOnly = searcher.PropertyNamesOnly,
                ReferralChasing = searcher.ReferralChasing,
                SearchRoot = searcher.SearchRoot,
                SearchScope = searcher.SearchScope,
                SecurityMasks = searcher.SecurityMasks,
                ServerPageTimeLimit = searcher.ServerPageTimeLimit,
                ServerTimeLimit = searcher.ServerTimeLimit,
                SizeLimit = searcher.SizeLimit,
                Tombstone = searcher.Tombstone,
                VirtualListView = searcher.VirtualListView
            };

            result.Filter = filter;
            if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(searcher.Filter) && searcher.Filter != "(objectClass=*)")
            {
                result.Filter = string.Format(CultureInfo.InvariantCulture, "(&{0}{1})", searcher.Filter, filter);
            }

            foreach (var property in searcher.PropertiesToLoad)
                result.PropertiesToLoad.Add(property);

            if (properties != null)
                result.PropertiesToLoad.AddRange(properties);

            return result;
        }
    }
}