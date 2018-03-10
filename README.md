# Senior Project 2018
A SPA application using VueJS/webpack and ASP.NET.
  
  How to start (assuming the use of vsCode):
    
    1. Open vscode and run git clone https://github.com/lladioll/SeniorProject.git from the command line.
    
  Now you can run the following commands:
    
    1. 'dotnet restore' in the base directory of the project
    2. 'cd client'
    3. 'npm install'
    4. 'cd ..'
    5. 'dotnet watch run'
    
Now your all set.  use dotnet watch run to run the server and watch for changes.  You can edit your client in the client/src folder and when you are ready to deploy the final version run the following commands:
 
    1. 'cd client'
    2. 'npm run build' (this is configured to copy the static files to /wwwroot folder).
    3. 'cd ..'
    4. 'dotnet publish -c release'

Your published files will be in bin\release.

