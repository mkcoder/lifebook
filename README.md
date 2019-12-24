# build status
[![Azure Build Status](https://dev.azure.com/mkmuhammadkhan/mk_muhammadkhan/_apis/build/status/mkcoder.lifebook?branchName=develop)](https://dev.azure.com/mkmuhammadkhan/mk_muhammadkhan/_build/latest?definitionId=1?branchName=develop)


# lifebook
web application to monitor and record the events of your life through: notes, calendar events, reminders, goal keeper, and other interesting features

---

# naming conventions followed throughout
## namespaces
lifebook.{solutionname}.{projectname}.{fodler}
<pre>
example:
lifebook
|_ core --- <b>* no namespace at this level ever [0] *</b>
  |_ logging <b>* lifebook.core.logging [1] *</b>
    |_ interface
    |_ services <b>* lifebook.core.logging.services [2]*</b>
    |_ ioc
      |_ activators <b>* lifebook.core.ioc.activators [3] *</b> 
<b>[0] we enforce/discourage the use of root level namespaces and restricted to very limited use</b>
<b>[1] this is reserved for project wide classes </b>
<b>[2|3] we prefix the namespace with folder </b>
lifebook.core.logging.interface
</pre>     

# project creation and development
## how to start a new project
* all apps must have their own wiki page, project home, and tags, and labels creted before inital project kickoff
> in the future apps maybe required to create their own pipeline 
a default solution may look something like this, please read more about different thing in the wiki page:
<pre>
basic layout of app solutions
| solution
| _ service
   | _ core
   | _ models
   | _ domains
   | _ valueobjects
   | _ configurations
   | _ repository
   | _ ioc
| _ webapi
   | _ apis
   | _ services
   | _ db
| _ webapp
   | _ app    
       | _ src
           | _ libs
           | _ js
           | _ css
           | _ resources
           | _ index.html
   | _ node_modules 
   | _ buildscripts
| _ businessprocess -- folder
   | _ <solutionname>.<purpose>.businessprocess 
      | _ service
      | _ ioc
      | _ aggregate
         | _ events
         | _ commands
      | _ controllers
      | _ models
      | _ domains
      | _ process.cs
      | _ start.cs    
| _ projectors
   | _ projections
   | _ projectors
   | _ api-controllers
   | _ start.cs
</pre>

# lifebook core framework support and packages
* lifebook.core.services
   * Configuration - adds configuration to the project, if it is a webapi project you could add a appsettings.json and we will pick it up
   * NetworkLocater 
      * look up service on consul
      * de/register service with consul
      * + get config file from consul
   * Hosting - Add webapi support to project
      * Creates a webserver 
      * Register's service to consul
      * Setup container
      * Setup configuration        
