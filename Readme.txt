1. Please update your connection strings in appSettings.json.
2. Please ensure to run migration under pacakge manager console.
-- Update-Database
3. If you want to connect this api source to any front end change the Client_URL with your url in appsettings.json

//api's url :
1. to register user
POST : /api/ApplicationUsers/Register
exammple for model
Model :
{
    "UserName":"vivek",
    "Email":"v@k.com",
    "FullName":"vivek kumar",
    "Password":"hello"
}

2. to login user
POST : /api/ApplicationUsers/Login
example for model
Model : 
{
    "UserName":"vivek",
    "Password":"hello"
}
this api will return Auth token for the user.
example :
"token":{"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiIxNTkyNmI5MC0wOTZlLTQxYjgtODlhMy04MzJjZmQwMjVjMDEiLCJuYmYiOjE2MTAxODI2MDAsImV4cCI6MTYxMDI2OTAwMCwiaWF0IjoxNjEwMTgyNjAwfQ.HvBfYQ0Aej343WlhnPJAlE9sg62VCNgzSfryPGPzlYg"
}

3. to add blog
POST : /api/blogs
example model
Dont forget to add auth token returned from login.
Model :
{
    "Subject":"vikku blogaaaaaas 3",
    "Body":"<!DOCTYPE html><html><head><title>Page Title</title></head><body><h1>My First last</h1><p>My first paragraph.</p></body></html>"
}

4. to get blog
GET : /api/blogs/1
Dont forget to add auth token returned from login.

5. to update blog
PUT : /api/blogs/1
Dont forget to add auth token returned from login.
Model :
{
    "Id":1,
    "Subject":"vikku blogssss updated",
    "Body":"this is my blog updated ssasa"
}

6. to delete blog
DELETE : /api/blogs/1
Dont forget to add auth token returned from login.

7. to get all blogs for the login user.
GET : /api/blogs
