[News Feeds](News-Feeds)
## Welcome to LINQ-SQO and MinLINQ

This project helps you to understand how the Standard Query Operators of **LINQ to Objects** work. Those operators are a crucial part of the LINQ technology that's part of the .NET Framework 3.5 release, previously code-named "Orcas". For an overview of LINQ, please take a look at [http://msdn.microsoft.com/data/ref/linq](http://msdn.microsoft.com/data/ref/linq). The source code included with this project is based on the official specification for the .NET Standard Query Operators and is conform with the .NET 4 implementation ([release:38099](release_38099)).
## What are the .NET Standard Query Operators?

See [http://community.bartdesmet.net/blogs/bart/archive/tags/LINQ/default.aspx](http://community.bartdesmet.net/blogs/bart/archive/tags/LINQ/default.aspx) for more information.

## MinLINQ

MinLINQ is an implementation of the LINQ to Objects Standard Query Operators using a function-oriented layered approach. Based on three essential operators called Ana, Bind and Cata, others are implemented. While the current implementation focuses on IEnumerable<T> exclusively, the same layering can be used to a dual IObservable<T> implementation. For more information on MinLINQ, read my blog post "The Essence of LINQ - MinLINQ" at [http://blogs.bartdesmet.net/blogs/bart/archive/2010/01/01/the-essence-of-linq-minlinq.aspx](http://blogs.bartdesmet.net/blogs/bart/archive/2010/01/01/the-essence-of-linq-minlinq.aspx).

## Releases

The latest release is version 4.0, updated to be conform with the .NET Framework 4 release. You can find it here: [release:38099](release_38099). Enjoy!

## Notes

For those of you interested in expression trees and how to build your own LINQ query provider, take a look at my **[LINQ to AD sample](http://www.codeplex.com/LINQtoAD)**. Check out the **[LINQ to SharePoint project](http://www.codeplex.com/LINQtoSharePoint)** as well, which translates LINQ queries to CAML.

## About the project founder

A former Visual C# MVP, **Bart De Smet** now works at Microsoft Corporation as a software engineer. Prior to this new challenge, Bart was active in the Belgian community evangelizing various Microsoft technologies, most of the time focusing on CLR, language innovation and frameworks. In his evangelism role, he's been speaking at various events and attended several international conferences including TechEd Europe, IT Forum and the PDC. In 2005, Bart graduated as a Master of Informatics from Ghent University, Belgium. Two years later, Bart became a Master of Computer Science Software Engineering from the same university.

You can visit Bart's blog on [http://blogs.bartdesmet.net/bart](http://blogs.bartdesmet.net/bart)