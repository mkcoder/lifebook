This is the solution for all things related to orchestrator.

What are orchestrator:
    - These are simple things you want to automate.
    - Orchestrator react to different things throughout the various microservices:
        - Events
            - When event x is created do this
        - Api Calls
            - Log the user who called service x 
        - Services coming up/down
            - When service x is deregistered from Consul call me
            - When service x is registered to consul more than once send an email
