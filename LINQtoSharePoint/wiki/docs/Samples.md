**+Note+: This page contains sample code based on the 0.2.0.0 release and earlier. Some samples don't work with the 0.2.2.0 release anymore because of API changes. The samples page will be updated accordingly shortly.**

# Samples

This page contains a set of samples that illustrate how to use LINQ to SharePoint.

## Getting started

In order to take a jumpstart with LINQ to SharePoint, view the **Getting started with LINQ to SharePoint video** ([C#](Samples_LINQtoSharePoint_CS.wmv) - [Visual Basic](Samples_LINQtoSharePoint_VB.wmv)).

### General information

Before you start, download binaries and check the machine configuration:
# Download the **LINQ to SharePoint binaries** from CodePlex (see Releases tab). Check for an update regularly.
# Download and install the **Windows SharePoint Services 3.0 SDK** from [http://www.microsoft.com/downloads/details.aspx?familyid=05E0DD12-8394-402B-8936-A07FE8AFAFFD&displaylang=en](http://www.microsoft.com/downloads/details.aspx?familyid=05E0DD12-8394-402B-8936-A07FE8AFAFFD&displaylang=en)

LINQ to SharePoint is built around the following concepts:
* **Entity objects** represent rows from SharePoint lists in a strongly-typed fashion. Entity type definitions can be exported from a SharePoint list definition using the **[SpMetal](SPMetal)** tool that comes with LINQ to SharePoint.
* A SharePoint list is characterized as a **[SharePointDataSource<T>](SharePointDataSource)** in LINQ to SharePoint, where T is an entity type as explained above.

To use LINQ to SharePoint in your .NET 3.5 project:
# Add a reference to _BdsSoft.SharePoint.Linq.dll_ and to _Microsoft.SharePoint.dll_ of the Windows SharePoint Services Object Model.
# Drag-and-drop entity type definition files that were generated using [SpMetal](SPMetal) from Windows Explorer to the project node in Solution Explorer.
# Create a [SharePointDataSource<T>](SharePointDataSource) object to write queries against the SharePoint list. Import the _BdsSoft.SharePoint.Linq_ namespace to access the [SharePointDataSource<T>](SharePointDataSource) type.

### A first sample

The following piece of C# 3.0 code shows how to write a LINQ query against a SharePoint data source using LINQ to SharePoint:

{{
using System;
using BdsSoft.SharePoint.Linq;

class Program
{
   static void Main()
   {
      var users = new SharePointDataSource<User>(new Uri("http://www.mysite.com"));
      var res = from u in users
                orderby u.MemberSince descending
                where u.Age >= 24 && u.FirstName.StartsWith("B")
                select new { Name = u.FirstName + " " + u.LastName, u.Age, u.MemberSince };

      foreach (var u in res)
         Console.WriteLine(u);
   }
}
}}
**Tip:** To examine the CAML queries and additional query information at runtime, use the [SharePointDataSource<T>](SharePointDataSource)'s Log property as shown below:

{{
      var users = new SharePointDataSource<User>(new Uri("http://www.mysite.com"));
      users.Log = Console.Out;

      var res = from u in users
                ...
}}

### Under the covers

The CAML query for the sample above looks as follows:

{{
<Query>
  <Where>
    <And>
      <Geq>
        <Value Type="Number">24</Value>
        <FieldRef Name="Age" />
      </Geq>
      <BeginsWith>
        <FieldRef Name="First_x0020_name" />
        <Value Type="Text">B</Value>
      </BeginsWith>
    </And>
  </Where>
  <OrderBy>
    <FieldRef Name="Member_x0020_since" Ascending="FALSE" />
  </OrderBy>
</Query>
<ViewFields>
  <FieldRef Name="First_x0020_name" />
  <FieldRef Name="Last_x0020_name" />
  <FieldRef Name="Age" />
  <FieldRef Name="Member_x0020_since" />
</ViewFields>
}}
Notice that the projection results in the creation of a <ViewFields> element that restricts the columns returned by the query.

## Guidelines for writing queries

### Supported operations

LINQ to SharePoint implements a subset of the [Query schema of CAML](http://msdn2.microsoft.com/en-us/library/ms467521.aspx). It supports the following CAML elements:
* Query
	* Where
		* **Logical joins**
			* And
			* Or
		* **Comparison Operators**
			* BeginsWith
			* Contains
			* Eq
			* Neq
			* Gt
			* Geq
			* Lt
			* Leq
			* IsNull
			* IsNotNull
	* OrderBy

There's no support for the [DateRangesOverlap](http://msdn2.microsoft.com/en-us/library/ms436080.aspx) and [GroupBy](http://msdn2.microsoft.com/en-us/library/ms415157.aspx) elements.

### Writing valid conditions

LINQ to SharePoint requires leaf-level conditions (i.e. conditions without Boolean operators) to written in a fixed format with only one reference to an **entity type property**. Valid examples include:

{{
u.FirstName == "Bart"
u.Age >= 24
1234 < u.AccountBalance
u.FirstName.StartsWith("B")
}}
It's invalid to have more than one entity property reference in a leaf-level condition, like this:

{{
u.Age < u.DoubleAge
u.FirstName.Contains(u.NickName)
}}
All **calculations** should occur on the value side of the condition. The following condition is valid:

{{
u.Age < 2 * someVariable
}}
but the next one isn't valid:

{{
u.Age / 2 < someVariable
}}
### Inverse order

Conditions in LINQ can be written in reverse order, like this:

{{
24 <= u.Age
"Bart" == u.Name
true != u.IsMember
}}
LINQ to SharePoint will reverse the order of the condition operandi automatically before making the translation to CAML. This is required because CAML conditions always compare the [FieldRef](http://msdn2.microsoft.com/en-us/library/ms442728.aspx) with the [Value](http://msdn2.microsoft.com/en-us/library/ms441886.aspx) in that order.

### String operations

The following methods on System.String are supported in LINQ to SharePoint:

* **StartsWith** (<BeginsWith>...</BeginsWith>)
* **Contains** (<Contains>...</Contains>)
* **Equals** (<Eq>...</Eq>)

Excessive **ToString** calls are stripped off automatically when using == or != comparisons:

* u.FirstName.ToString().ToString() == "Bart" becomes u.FirstName == "Bart"

The entity property reference should always occur on the left-hand side of the condition when using these methods:

{{
u.FirstName.StartsWith("B")
u.LastName.Contains("Smet")
u.City.Equals("Ghent")
}}
The operator overloads **==** and **!=** are supported too and have an equivalent meaning as Equals or its negation.

### Nullable types

Entity properties that have been marked as Nullable because it aren't reference types and the field is not defined as required in the SharePoint list definition can be checked for null values in two ways:

{{
u.Age.HasValue
u.Age != null
}}
To reference the value of the nullable property, two approaches exist as well:

{{
u.Age == 24
u.Age.Value == 24
}}
In Visual Basic, the second approach has to be followed, while C# provides more flexibility so that you can drop the .Value property call.

### Working with Choice and MultiChoice fields

Choice and MultiChoice fields are mapped on enum types by the [SpMetal](SPMetal) tool. Each [CHOICE](http://msdn2.microsoft.com/en-us/library/ms439235.aspx) from SharePoint is mapped on a field in the target enumeration, possibly decorated with a **ChoiceAttribute** to indicate a different underlying name (for example {"[Choice("Laurel & Hardy")](Choice(_Laurel-&-Hardy_))"} will be applied on an enum field LaurelHardy). MultiChoice fields (represented by radio buttons in SharePoint) are mapped on a {"[Flags](Flags)"} enumeration where all values are powers of two to allow bitwise combination. Examples are shown below:

{{
[Flags](Flags)
enum FavoriteFood
{
   Pizza = 1,
   Lasagna = 2,
   Hamburger = 4
}

enum MembershipType
{
   Gold,
   Silver,
   Bronze
}
}}

Conditions on **Choice fields** should look like this:

{{
u.MembershipType == MembershipType.Silver
u.MembershipType != MembershipType.Gold
}}
and are translated into <Eq> or <Neq> CAML conditions. Comparison operators like <, <=, > and >= won't trigger compilation or runtime errors but shouldn't be used.

Conditions on **MultiChoice fields** should look like this:

{{
u.FavoriteFood == FavoriteFood.Pizza
u.FavoriteFood != FavoriteFood.Pizza
u.FavoriteFood == (FavoriteFood.Pizza | FavoriteFood.Lasagna)
}}
**Warning:** There's a semantic mismatch between LINQ queries for MultiChoice fields and what you normally expect in C#.
* The first condition means that Pizza should be +one of+ the choices applied for the list item. To that respect, it's equivalent to the _(u.FavoriteFood & FavoriteFood.Pizza) == FavoriteFood.Pizza_ syntax normally used to check enumeration flags. This syntax isn't supported though.
* In a similar fashion, the second condition means that Pizza shouldn't be in the list of favorite foods of the list item; it doesn't restrict any other values though.
* The last condition is equivalent to _u.FavoriteFood == FavoriteFood.Pizza || u.FavoriteFood == FavoriteFood.Lasagna_ but again it doesn't rule out the presence of other choices on the list item.
If you want to use an absolute equality check, it should be written manually. For example, to find people who only like Pizza (and nothing but that), you'd have to write _u.FavoriteFood == FavoriteFood.Pizza && u.FavoriteFood != FavoriteFood.Lasagna && u.FavoriteFood != FavoriteFood.Hamburger_.

Fields with **fill-in choices** will have an additional mapping field of type string that can be null (no fill-in choice made) or set to some string. This mapping field is cross-linked from the original field using the **OtherChoice** property of the **FieldAttribute** mapping, like this:

{{
    /// <summary>
    /// Favorite food
    /// </summary>
    [Field("Favorite food", FieldType.MultiChoice, Id = "c48610a1-098e-438c-9f77-6e65c6a392cb", OtherChoice = "FavoriteFoodOther")](Field(_Favorite-food_,-FieldType.MultiChoice,-Id-=-_c48610a1-098e-438c-9f77-6e65c6a392cb_,-OtherChoice-=-_FavoriteFoodOther_))
    public FavoriteFood? FavoriteFood { get; set; }

    /// <summary>
    /// Favorite food 'Fill-in' value
    /// </summary>
    [Field("Favorite food", FieldType.Text, Id = "c48610a1-098e-438c-9f77-6e65c6a392cb")](Field(_Favorite-food_,-FieldType.Text,-Id-=-_c48610a1-098e-438c-9f77-6e65c6a392cb_))
    public string FavoriteFoodOther { get; set; }
}}

The fill-in choice entity property can used in queries too:

{{
u.FavoriteFoodOther == "Steak"
u.FavoriteFoodOther != "Steak"
}}
Again, the semantic mismatch applies and putting a condition on the fill-in choice doesn't say anything about the possible presence of other choices. Therefore, the first condition will retrieve all the people who like Steak but necessarily +only+ Steak. The last condition retrieves everyone who doesn't like Steak.

In the current implementation, the fill-in choice field can also be used to put restrictions on known choice values. For example, you could rewrite _u.FavoriteFood == FavoriteFood.Pizza_ with _u.FavoriteFoodOther == "Pizza"_. To this respect, the 'Other' suffix on fill-in choice fields is a bit of a misnomer. This flexibility allows the list definition to be extended with new CHOICE values, without having to change the code. For example, u.FavoriteFoodOther == "Steak" will keep working even when Steak is added as a recognized pre-defined CHOICE value on the field.

### Boolean negation

Boolean negation isn't supported directly in CAML, but LINQ to SharePoint knows how to invert most of the supported comparison operators and implements [De Morgan's laws](http://en.wikipedia.org/wiki/De_Morgan's_laws) to transform Boolean conditions with negations into a negation-less equivalent. A few examples:

* !(u.Age == 24) becomes u.Age != 24 (<Neq>...</Neq>)
* !(u.Age <= 24) becomes u.Age > 24 (<Lt>...</Lt>)
* !(u.Age > 24 && u.FirstName == "Bart") becomes u.Age <= 24 || u.FirstName != "Bart" (<Or><Leq>...</Leq><Neq>...</Neq></Or>)
* !(u.Age >= 24 || !(u.FirstName == "Bart" && u.AccountBalance < 1234)) becomes u.Age < 24 && (u.FirstName == "Bart" && u.AccountBalance < 1234) (<And><Lt>...</Lt><And><Eq>...</Eq><Lt>...</Lt></And></And>)

Negation of the BeginsWith and Contains operators isn't supported though.