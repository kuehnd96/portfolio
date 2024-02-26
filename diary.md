# Diary of my journey with portfolio project

### Come along as I plan, design, implement, and support software for a massive increase in my online presence

2/25/2024

I come to this diary entry today with a ton of enthusiam and drive for this project. I have been thinking a ton about this project and am going to change my plan from getting the site all set up with all of the content I have been planning to a more narrow goal of getting one vertical slice of the project done including the use of AI. The best term for this type of approach that I have read about is "tracer bullets". I will still have to do all of the fun setup of standing up a new solution, come up with cloud infrastructure, and setup of all of my builds and deployments but gets my hands dirty with all of the aspects of my architecture much sooner. My recent spike in enthusiasm for this project is rooted in this new approach and I am excited to get going on this. Right after pushing this diary update I am going to get busy planning what needs to be done and filling my backlog. I think this approach of standing up one feature of this project will allow me to fail faster, find solutions to problems with all parts of the solution quicker, and get to getting something out there quicker. I am off to start moving on this.

11/6/2023

Today is the last day of .NET Conf. A new LTS version of .NET is out and I am excited to try out some of the new things that were announced. A new hosting model for Blazor was introduced and I think it will be perfect for the type of Blazor app I plan to build for this project: Static server side rendering. It features the ability to run a Blazor app server side and pump HTML to the browser without the need for a persistant SignalR connection. I will be updating my ADR with the desion to use this hosing model.

My excitement for this project is about as high as it has ever been. Next up I will finish planning my content, creating a site map, and composing and designing my data for persisted storage. After that I will learn some Blazor and then get coding. Also, I think I found a way to get some time every week to work on this project. I should be able to stick to this new schedule even with the holidays coming up.

10/29/2023

The good news is I am 4 weeks into my new job and loving it. The bad news is I didn't have time to work on this project during my job search. Even though it has taken until now to post my first update in this diary I have been thinking about this project and doing some work on it for a couple weeks. 

Fisrt off, the biggest task right now is to learn more about Blazor. I am using [this repo](https://github.com/kuehnd96/learning_blazor) as a sandbox for tinkering with Blazor as I read my book. I just finished creating a dev container for this repo so I can now run this code in a browser or any computer without needing to set up the dev environment.

Second, I need to complete designing the content for the site. This includes deciding what I want to show, creating the site map, and designing the data for persisted storage. Thankfully, I am getting close on this and can be worked on it smaller chunks.

Third, there are a few technical things I need to do to make sure I am set up for some longer-term technical plans for this project. More on this in the coming months.

These next two months will be busy with some travel and holidays but I will find some time for this project.

8/1/2023

Just when I am establishing some consistency and momentum on this project something comes along and disrupts it ... temporarily. My employer (Rocket Mortage) announced voluntary severance packages to get their head count down due to the slow mortgage industry and I decided to take it. I have already started looking for a new job and, at first, thought this initative would take up too much time to keep going on this side project. I soon realized that once I am done at Rocket Mortgage and through a couple of week-long family vacations this month I should have MORE time to work on this project than I have had. Looking for a job is like a job in itself but I shouldn't do that for 40 hours a week. Plus, working on this portfolio site would be a great thing to talk about with potential employers. I am now excited about having more time to work on this project in a few weeks on top of the excitement I have for finding my next job as a software developer.

6/18/2023

Well, that was a big gap in updates on this project. Here is what I have been up to:

I have been updating the ADR file. The ADR file holds the place where I compose my architecture decisions regarding this project. The updates to this file are the tip of the iceberg as I have been thinking a lot about the technical trade-offs of this project. I am near completion of the ADR file. The last two decisions are CSS framework and how much data is served by persisted storage and an API. I am leaning heavily toward bootstrap for the CSS since I like using that with my [Angular training](https://github.com/kuehnd96/rocket_hackweek_Q1_2022) last year more than my experience with Tailwind in a green-field C# project with Angular back in 2021 for a consulting client. The other decision brings me to another noteworthy thing that happened in the last few months.

In late May I consumed Microsoft's Build Developer Conference virtually from my home office. I consumed a lot of content on AI and what Microsoft offers in this area. My mind started racing about how AI could be applied to my work at Rocket Mortgage and to this side project. I hatched an idea that would require data of my career experience to be served by an API. Perhaps this is the way to go right away with this portfolio site? This way I can set myself up later for more ways of using this data? I am leaning toward this as my decision regarding this static data/persisted storage data decision. Either way, I am chomping at the bit to get my hands dirty with Microsoft's newer AI offerings. This would be phase two of this project.

The other thing I have been doing is making progress in my [Blazor book](https://www.oreilly.com/library/view/learning-blazor/9781098113230/). While admitting this progess has been tiny I am encouraged by a new desire to work on this project coupled with increased time to do it since it now summer time. Now that the busiest time of the year as a parent is over I will hold myself to making a lot more progress on this project in the next few months than I have since in the 5 or so months since I hatched this project. I invite anyone reading this to hold me to this, too.

2/7/2023

I know I have not updated this in a while. There is good news and bad news on this front:

The bad news is I have been dealing with the sudden loss of my uncle. This caused me to shift my focus away from this project for a few weeks to grieve and center myself. I am finding this has been the proper way to deal with this loss.

The good news is I started ready my [Blazor book](https://www.oreilly.com/library/view/learning-blazor/9781098113230/) and I am really liking it. I am looking to get into a more consistent pattern of reading this book every week. I also need to do some research on how to host a blog in Azure. This project is still throught about often and I haven't lost any excitement about it.

1/9/2023

Happy New Year. After taking some time away from software development outside of work I am ready to get back to it. I am questioning my previous choice of Gatsby (React) and Contentful for this portfolio site. I am now leaning heavily toward .NET Blazor and Azure to create this site and my blog. The reason is I think using the .NET stack and Azure would be more fun and a better use of my time. I don't see myself needing any more React experience in my side projects or my job. The more I think about going in this new technical direction the more my excitement and enthusiasm grows for this project. This would be a great opportunity to level up in the new features, tools, and frameworks of .NET. I have spent the last few years gaining technical breadth outside of .NET outside of work; The idea of gaining technical depth in .NET from this project is too intriguing.  

12/14/2022

With my study of object-oriented software development patterns complete I can turn all of my attention to building this portfolio project ... just in time to take a break from side projects until after Christmas. A little break from being constructive in this way is needed to recharge a bit and prepare for the holidays. I have done a lot this year (working to get busy light working with my Raspberry Pi, teaching myself to create and use dev containers, [Angular training](https://github.com/kuehnd96/rocket_hackweek_Q1_2022), and design pattern study) and a little break is needed to get back to things in a week and a half. There is no doubt this project will be on my mind during this break. I am anxioux to get to filling out the ADR file in this repo. Happy holidays! 🎄 

12/4/2022

I have been doing some reading about Gatsby this weekend. This looks to be the right level of static content speed, CMS integration, and flexibility to create dynamic HTML that I am looking for. I like how it offers high performance, easy integration with CMS's (for my blog), and the familiarity of React. (I am glad I taught myself some React a few years ago outside of work.) Next up are some technical exercises with Gatsby, Azure static web sites, Contentful, and finding the right way to integrate my blog into the portfolio site.

11/27/2022

With my study of object-oriented software design patterns winding down the amount of time I have been thinking about this project has been going way up. I have been researching the portfolio sites of others for design inspiration and doing some research on Gatsby in regards to my options for creating a blog as part of this portfolio site. I will keep this updated with my progress and status. Happy (belated) Thanksgiving to all those who celebrate in the United State.
