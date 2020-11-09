## Introduction
LINQ to Active Directory implements a custom LINQ query provider that allows querying objects in Active Directory. Internally, queries are translated into LDAP filters which are sent to the server using the System.DirectoryServices .NET Framework library. LINQ stands for Language Integrated Query and is one of the core features of Microsoft's .NET Framework 3.5 release. More information can be found via the MSDN website on [http://msdn.microsoft.com](http://msdn.microsoft.com).

## Features

* Translates into **LDAP filters** according to [RFC 2254](http://www.faqs.org/rfcs/rfc2254.html).
* Simple and approachable **entity model** with support of propagating **updates** back.
* Supports mappings to both the **System.DirectoryServices** (.NET) and **ActiveDs** (COM) APIs.
* Ships with a set of **samples**.

## Disclaimer

This project is meant as a basic sample on implementing custom LINQ query providers. It hasn't been tested thoroughly and we do not provide any support whatsoever. Do not use it in a production environment without proper testing and validation of the technology's behavior. Users are most welcome to report issues and bugs through the Issue Tracker on this site.

## Samples

### C# sample

{{
    // NOTE: Entity type definition "User" omitted in sample - see samples in release.

    var users = new DirectorySource<User>(ROOT, SearchScope.Subtree);
    users.Log = Console.Out;

    var res = from usr in users
              where usr.FirstName.StartsWith("B") && usr.Office == "2525"
              select new { Name = usr.FirstName + " " + usr.LastName, usr.Office, usr.LogonCount };

    foreach (var u in res)
    {
        Console.WriteLine(u);
        u.Office = "5252";
        u.SetPassword(pwd);
    }

    users.Update();
}}

### Visual Basic sample

{{
    ' NOTE: Entity type definition "User" omitted in sample - see samples in release.

    Dim users As New DirectorySource(Of User)(ROOT, SearchScope.Subtree)
    users.Log = Console.Out

    Dim res = From usr In users
              Where usr.FirstName.StartsWith("B") And usr.Office = "2525"
              Select New With { Name = usr.FirstName + " " + usr.LastName, usr.Office, usr.LogonCount }

    For Each u In res
        Console.WriteLine(u)
        u.Office = "5252"
        u.SetPassword(pwd)
    Next

    users.Update()
}}

## About the project founder

A former Visual C# MVP, **Bart De Smet** now works at Microsoft Corporation on the WPF dev team in an SDE role. Prior to this new challenge, Bart was active in the Belgian community evangelizing various Microsoft technologies, most of the time focusing on CLR, language innovation and frameworks. In his evangelism role, he's been speaking at various events and attended several international conferences including TechEd Europe, IT Forum and the PDC. In 2005, Bart graduated as a Master of Informatics from Ghent University, Belgium. Two years later, Bart became a Master of Computer Science Software Engineering from the same university.

You can visit Bart's blog on [http://blogs.bartdesmet.net/bart](http://blogs.bartdesmet.net/bart)