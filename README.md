# DistantEdu
Web-application designed for creating small lessons with tests. There are two types of users now: teachers and students. Teachers are creating lessons and tests, and students are passing it. Works with MSSQL Server and Duende IdentityServer

## Description
This was developing as lab. project for BSUIR. It has to contain two patters (and now contains useless Singleton and useful Mediator).
Here are used MediatR + CQRS to segregate business logic from controllers. 

For now there are 2 types of users devided by roles: 
+ Students
+ Teachers

Teachers are able to create subjects, lessons and tests (quizzes). Students are passing it.
Quiz creation form contains fields for duration, questions count beside test's name and description.
Quizzes are created to be more random. E.g. quiz has Question count field. If there are more available questions, than only specified count will be randomly selected from all available questions.
This feature was implemented also for replies.
There is also features for "fast fill" for questions and "download". The first enables to create questions from text, the second downloads all questions it txt file and mey be used then for fast fill.
