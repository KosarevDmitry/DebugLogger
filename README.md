In my opinion, this is my first finding that solved my problems with aspnetcore, reduced the stress in understanding the process, allowed me to calm down, the process of tracking the flow has become more controlled, optimized one.

The fact is that the process of creating a webapi app consists of two parts at least.
First of all it is worth mentioning that the whole process resemebles factory conveyor.
I myself worked on conveyor little bit being a student.
The conveyor belt is moving, each worker performs a short simple operation, for example, I inserted insoles into shoes.
This was our practice to get acquainted with factory.
First, the worker must stock up on these insoles so that they are at hand.
He must get them from the warehouse, unpack the box, well, something like that.
Then the line is turned on, the conveyor belt runs.
The first process in the assembly before the request goes into work
is the creation of a mass of services.
And when you look at these operations, then, frankly, nothing is clear.
It's like opening Lego packages.
I want it to be clear, not just that the property, service  were added
but why, why exactly this way and why it is needed at all.
The questions will still remain.
But still, it is easier to understand if you track when which service is attached  and where.

Familiarity with the Protocols makes it easier to understand.
After all, there is nothing mysterious there, eventually.
We agreed that the response should have a header of such type and its length and value can be such and such.
Why exactly this name, we agreed and wrote it down in a document that everyone follows
that is, no matter how difficult it may seem first, you need to remember that the input is always http request
the output is always http response. It is easier to get used to the appearance of these entities using js, for example `expresjs`
because the text format is very visual, and in dotnetcore there are already abstractions
it is effective but what this final thing looks like is not clear there you need to constantly monitor it.
In principle it is not so difficult, having understood that at the beginning an object is created, called httpcontext and to it are added
request and response and a set of all the features, properties.
That is, in principle, you can serialize them and see what happened at this stage.
what does this blank look like that the facrory lathe work with. or speaking about the conveyor, if there are insoles, have they already been put there?
how many are there? For some reason they put 2 pieces in each shoe, which is strange, but nevertheless there is some logic there.

What I wrote about my problems, I solved it by adding my logger, that is, one that shows where the recording took place and my personal
description, although not perfect, is already something. 

The thing is that at first I made comments in the aspnetcore repo somehow tried to capture reality, describe it,
conditionally speaking, what I see. Why this class and so on or just comments, for example, there are pieces of code that I would like to have in my arsenal
then this is essentially a snippet with an algorithm that is understandable to me and can be encountered again and than reinvent the wheel, of course, it is better to take this implementation
that is, extract it from the entire bunch of classes and save it somewhere.
The work was not very successful.


Then I decided to do logging, that is, pulling my comments out with a time reference.
The sequence of events is important as the event itself.
I  created a project with a static class and added this one to my local repository https://github.com/dotnet/aspnetcore.git. to make it easier to understand its processes. 

Comments are logged in markdawn That is, you need to understand that  from standard standpoint `callers` are not used  usually becouse it is slow approach,  but for debugging it is exactly that I wanted.

It can be added as package but I made it that way.

Add to repository

I have tried to add a project as it is recommended in `\docs\ReferenceResolution.md`
but there are some peculiarities

Actually

1. add project (it is obvious) in dir  `\src\MyProject`.  Add dir `src`, as this is s common pattern so the path will end up being `\src\MyProject\src`

2. add  in D:\src\aspnetcore\eng\Build.props
 
<DotNetProjects Include="  
     $(RepoRoot)src\MyProject\**\src\*.csproj;
	 where `$(BuildMainlyReferenceProviders)' == 'true'`

3. run  `eng/scripts/GenerateProjectList.ps1`
4. add  in `eng\Dependencies.props`
in group `<ItemGroup Label=".NET team dependencies (Non-source-build)" Condition="'$(DotNetBuildFromSource)' != 'true'">`
`<LatestPackageReference Include="MyProject" />`
5. `\src\restore.cmd`

6. `dotnet sln AspNetCore.sln add .\src\MyProject\src`
7. Add scripts to `\src\MyProject\` to work with new project 

build.cmd
```
@ECHO OFF
SET RepoRoot=%~dp0..\..

%RepoRoot%\eng\build.cmd -projects %~dp0**\*.*proj %*
```

MyProject.slnf 
```
{
  "solution": {
    "path": "..\\..\\AspNetCore.sln",
    "projects": [
	  "src\\DebugLogger\\src\\MyProject.csproj"
    ]
  }
}
```

startvs.cmd

```
@ECHO OFF

%~dp0..\..\startvs.cmd %~dp0MyProject.slnf
```

8. Add dependency to all csproj  <Reference Include="MyProject" />. As my current repo version contains 618 csproj so usage of Directory.Build.Props to add reference is good idea. 
I am not sure that  `<TargetFrameworks>$(DefaultNetFxTargetFramework);netstandard2.0;$(DefaultNetCoreTargetFramework)</TargetFrameworks>`  fits to all cases.

Finish.


Log examples
```
DebugLogger.Logger.Log($"Redirect to accessDeniedUri {accessDeniedUri}");
DebugLogger.Logger.Log("Server.StopAsync");
```