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

