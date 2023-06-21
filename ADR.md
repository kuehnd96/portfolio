# Architecture Decision Report

### This file will contain details of the architecture decisions made for this portfolio project. Text in *italics* indicate decisions that I am noodling on.

## Web Stack

In the several months following the conception of this project I had my mind set on using Gatsby and Typescript as the web technology that would bring this portfolio web site to life. After doing some research on Gatsby and deciding that is was the technology that I needed for the mix of static and dynamic content for this project I started thinking about using .NET instead. The more I thought about this the more one simple thought kept ringing in my head. It was the thought that I like C# and dotnet more than any other tech stack that I have used and that using it on this project was the perfect opportunity to level up in C# and to add the arrow of Blazor to my Microsoft tech stack quiver. The decision to go with .NET instead of Gatsby and Node brought a huge wave of excitement and enthusiasm to this project for me. I still plan on learning Gatsby at some point but I am looking forward to slinging of C#.

### Blazor hosing model

After reading a great [resource](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-7.0&source=docs) on the 3 different hosting models for Blazor I have decided to create this project using the Blazor WebAssembly hosting model. This will sacrifice some performance on initial load and limit my .NET compatibility some. This seems like the way to go since this project will be a relatively thin web site with a lot of static content and I would like it to work when the browser is offline.

I should give Blazor Hybrid an honorable mention here. This is due to the fact that I would like to enable the use of this portfolio site on native devices (Windows, Mac, iPhone, iPad, and Android) using Microsoft's new MAUI technology. Razor components can be hosted this way without change regardless of what hosting model is chosen.

## Tools

### IDE: Rider, VS Code, or Visual Studio?

I have been using Visual Studio since 2002 and I am still a big fan of it. It has everything needed and more for developing this project. It is a bit heavy-handed these days due to how big (resource use, many tools and extensions not needed all the time) it is. 

I have not used Jetbrains Rider yet but have heard great things about it from those who rely on it. I still plan to try it one day but I am not looking to learn a new IDE as part of this project, though.

I have been using VS Code more and more for .NET work in the past few years to see how it compares with Visual Studio for .NET development. I have been liking it due to its low resource use and the reliance on the command line that it comes with.  (I have been enjoying returning to the command line after about 25 years; It reminds me of my days of using DOS for programming.)

I will use a mixture of VS Code and Visual Studio for this project. I will use Visual Studio when using my desktop PC and VS Code when I am using my (low-powered) Surface PC or my Mac.

## CSS

I have a little experience with both Tailwind and Bootstrap. I used Tailwind on a new .NET C# project for a consulting client in 2021. I used Bootstrap for styling the [Angular app](https://github.com/kuehnd96/rocket_hackweek_Q1_2022) I created for the Udemy course I used to level up in Angular in 2022. I liked the experience with Bootstrap more due to the ease of finding the styles I needed and how easy it was to compose markup with the style you want to apply. Bootstrap will be used for this project.

## **Cloud Hosting**

### Provider: Azure or AWS

On one had I have been using Azure since 2015 and have a (soon to expire) certification for the platform. I really like Azure and have no big problems with this provider.

On the other hand I have been using AWS since 2021 and use this provider at work. A designed, coded, tested, and deployed my first Lambda function (C#) and really enjoyed the experience due to the tools, testability, and serverless environment. I could always use more experience in AWS since it could help me in my job.

I have been waffling on this decision for weeks now but I have decided to use Azure. I will continue to get more experience in AWS through my job so I will balance the scale with some Azure for this side project. Diagrams are coming soon that will show what Azure offerings will be used; I have been thinking about this and doing a little research.
