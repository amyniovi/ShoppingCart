# README #


### What is this repository for? ###

This is a small basic shopping cart application
The server is a Web api 
The client is implemented in pure vanilla js (my technology of choice would have been Angular.js,
 the scripts are really bad code, not the way real apps should be written as its not scalable and object oriented. 
 I however did not have time to teach myself React for this project.)
 I could have used Mvc but thought it was interesting to use JS , mostly did it for me ;) but i guess i may have taken the longer route..


### How do I get set up? ###

Set PerfectChannelShoppingCart as the startup project
Run the application
enter a username (That is merely a way to access shopping carts, and its not Authentication or Authorization)
Start adding and removing objects from your basket. 
try adding more objects than available in stock
Try adding "oranges" that are not in stock
Checkout and get your invoice

thats about it.

Please also run the unit tests in PerfectChannelShoppingCart.Tests as these were my initial approach to the project (TDD)
 and my line of thought for the user stories


### Who do I talk to? ###

amyniovi@gmail.com

###Assumptions: ###
Username Login is implemented(no password) as it is needed to identify the user and cart.
Usernames Should be unique in this system.
No authentication is implemented.
The solution is not thread safe. Our item collection in ItemRepo is not thread safe.  
Havent implemented adding hypermedia support 
Havent set the links for the resources in this application apart..from a basic Uri string in the Item  class, which I am setting in the ItemController.
When sending the item collection to the client in a real application I would NOT send things like "stock" etc. 

 Vanilla JS scripts should be refactored to NOT use Global Vars, (use IIFE) and add event listeners to the elements in the page, however 
 that as well would need more time spent. 

