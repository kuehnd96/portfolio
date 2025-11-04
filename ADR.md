# Architecture Decision Report

### This file contains details of the architecture decisions made for this portfolio project. Text in *italics* indicate decisions that I am noodling on.

## Web Stack

In the several months following the conception of this project I had my mind set on using Gatsby and Typescript as the web technology that would bring this portfolio web site to life. After doing some research on Gatsby and deciding that is was the technology that I needed for the mix of static and dynamic content for this project I started thinking about using .NET instead. The more I thought about this the more one simple thought kept ringing in my head. It was the thought that I like C# and dotnet more than any other tech stack that I have used and that using it on this project was the perfect opportunity to level up in C# and to add the arrow of Blazor to my Microsoft tech stack quiver. The decision to go with .NET instead of Gatsby and Node brought a huge wave of excitement and enthusiasm to this project for me. I still plan on learning Gatsby at some point but I am looking forward to slinging of C#.

I am leaning heavily toward using React for the front end of this project instead of Blazor. There is more demand for React out there right now and I will be using it in my current job. The bottom line is I could use some more experience with React and TypeScript. I still plan to learn Blazor and recreating this app in Blazor could be a great way to do that.

### .NET

After a little research I have decided to use .NET 9 for this project. This is a standard-term support release which means I have 18 months of time before I have to update to the next verson of .NET (10). While this is a shorter window than with .NET 8 I am anxious to get more experience updating .NET apps between major versions. (At the time of this writing I am upgrading a few .NET 6 solutions to .NET 8 at work.)

I will take time in 2026 to upgrade this project to .NET 10. This will add to my experience with major upgrades of .NET versions.


## Tools

### IDE: Rider, VS Code, or Visual Studio?

I have been using Visual Studio since 2002 and I am still a big fan of it. It has everything needed and more for developing this project. It is a bit heavy-handed these days due to how big (resource use, many tools and extensions not needed all the time) it is. 

I have not used Jetbrains Rider yet but have heard great things about it from those who rely on it. I still plan to try it one day but I am not looking to learn a new IDE as part of this project, though.

I have been using VS Code more and more for .NET work in the past few years to see how it compares with Visual Studio for .NET development. I have been liking it due to its low resource use and the reliance on the command line that it comes with.  (I have been enjoying returning to the command line after about 25 years; It reminds me of my days of using DOS for programming.)

I will use a mixture of VS Code and Visual Studio for this project. I will use Visual Studio when using my desktop PC and VS Code when I am using my (low-powered) Surface PC or my Mac.

### CI/CD

I am going to go with Azure DevOps for deployment pipelines with this project since I have more experience with it. I am normally up for using side projects as an opportunity to learn new tools but I think that dance card is already full enough. I still want to learn GitHub issues and projects at some point but I not this time.

### Source Control

GitHub (I ❤ Git)

## CSS

I have a little experience with both Tailwind and Bootstrap. I used Tailwind on a new .NET C# project for a consulting client in 2021. I used Bootstrap for styling the [Angular app](https://github.com/kuehnd96/rocket_hackweek_Q1_2022) I created for the Udemy course I used to level up in Angular in 2022. I liked the experience with Bootstrap more due to the ease of finding the styles I needed and how easy it was to compose markup with the style you want to apply. Bootstrap will be used for this project.

## Cloud Provider

On one had I have been using Azure since 2015 and have a (soon to expire) certification for the platform. I really like Azure and have no big problems with this provider.

On the other hand I have been using AWS since 2021 and use this provider at work. A designed, coded, tested, and deployed my first Lambda function (C#) and really enjoyed the experience due to the tools, testability, and serverless environment. I could always use more experience in AWS since it could help me in my job.

I have been waffling on this decision for weeks now but I have decided to use Azure. I will continue to get more experience in AWS through my job so I will balance the scale with some Azure for this side project. Diagrams are coming soon that will show what Azure offerings will be used; I have been thinking about this and doing a little research.

### Web API Hosting

After some research I am planning to host the API in an Azure Container App. This lines right up with the cloud native serverless approach to this project and it will keep costs low due to the auto-scaling offered with this resource type. I am not expecting a lot of traffic on this site but I still considering this initiative as a lesson in scaling things properly and keeping costs down when it comes to using the cloud. Plus, using Azure Container App comes with the added bonus of upping my docker game with the creation of a production and dev container docker files.

At this point I don't see a risk or downside of going with Azure Container services. This doesn't mean that none exists, though. When I get to deploying my API to the cloud I will update this part of the ADR with any downsides that I find.

With the change to React from Blazor I am still looking at an Azure Container service resource in the cloud. I will update this document if this changes.

## Architecture

After some research I have decided to go with [clean architecture](https://github.com/ardalis/CleanArchitecture) for this project. (Onion architecture is another name for clean architecture.) At the center of my solution is a core project for interfaces, business logic, and model objects. All other projects will depend on this project. Other projects include infrastructure projects which will house different implementations of interfaces and "head" projects which include the API project and web UI project.

I have decided to not use the vertical feature slice arrangement of projects since it leads to large single projects and I would rather keep my projects smaller and separated. (I am not a fan of monoliths but plan to do some research on modular monoliths outside of this initiative.)

Dependency injection will be heavily used and I will keep this ADR up to date on which design patterns I adopt for use.

I decided in late 2024 to build a client SDK for the backend API in C#. On top of being a great technical challenge (even though I have built these before) it will serve as a proof of concept for the usefullness of these client SDK libraries. They wrap everything in a package and shield the invoker of the API from having to mess around with API configurations, URL's, request headers, and everything else that comes with calling an API.

## Persisted Storage

The persisted storage for the data that will power this portfolio will be stored in SQL Server. This fits since the data is not large or complicated and won't change often.

The data access layer of this solution will use a CQRS pattern with the micro ORM Dapper. The use of commands, handlers, and queries will provide separation of code that reads and writes data along with the level of testability and performance required.

Instead of using Entity Framework with migrations I have decided to use a SQL server project to represent the state of the database schema. I have used some sort of migrations so much in my career and I want to try something without migrations. After reading up on SQL server projects this sounds like the way to go due to the compatibility with CI/CD and its lack of migrations.