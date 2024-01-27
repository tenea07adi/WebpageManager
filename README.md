# WebpageManager
![cover](https://i.pinimg.com/736x/00/27/a6/0027a63871769ac7484ed0ad1966be76.jpg)
## About project
WebpageManager is a web system designed to give life to webpages. It is meant to be an admin panel (or data storage) integrated by simple webpages for the purpose to show continuous changing data.

## The project purpose
### Description:
As I mentioned before, WebpageManager should be a data source for websites. In order to do so, the system needs to:
 - be easily integrated by webpages
 - have an easy to use control pannel for users
 - have a consistent endpoint schema
 - have a consistent data storage system
 - have a proper security system

### Example:
 Let's take the following example: 
   - Problem: We want to have a small blog page to show our projects and thoughts. To achive this, we need a webpage and a way to manage the blog posts shared on it. For a project like this we have a few options, to create a webapp, a simple html page and update it every time or, use a system like WordPress (not our choice).
   - Solution: The development of a webapp blog needs a lot of time and to change a HTML webpage every time is not a solution. WebpageManager is created to figure out problems like that, we can simply create a webpage in WebpageManager, populate with blog posts (after we setup our data structure) and consume them on our html page.

## How it's working (project development)
### General Idea
WebpageManager is designed as a web REST API, built using ASP .NET Core. The main objective of the development (as a personal goal) was the creation of a simple arhitecture based on reuseble code. In order to do this, I designed reuseble components based on generics, secured by a token based authentication system.<br/> 
Additionaly, based on the web API, a user interface will be build using Vue.
### API Controllers Implementation
The API controllers are the way how the outside world interact with our system. It needs to be easy to understand, intuitive and secured. 

Text in progress...
### Security System Implementation
Text in progress...
### Data Retriveal Implementation
Text in progress...
### Models System Implementation
Text in progress...

## How to implement
### WebpageManager deploy
Text in progress...
### Data structure creation
Text in progress...
### Webpage implementation
Text in progress...

## Features
Below you can find the list of features implemented or in plan/working state

| Name | State |
| ---- | ----- |
| Endpoint authentication | Implemented |
| Users authentication and registration | Implemented |
| Webpage management | Implemented |
| Webpage simple info management | Implemented |
| Webpage advanced tables storage management | In development |
| User interface | In plan |
