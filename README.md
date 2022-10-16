## Stories
### As a user I want to create my profile with specified band role (instrument)
  1) A request will be handled by the endpoint '{userApiEndpoint}/api/v1/UserProfile/Create' HTTP.POST Method  (Everyone can create their user profile as they want)

        - Expected input
            Email : string, 
            Password : string, 
            UserType{'Host' or 'Player'} : Enum, 
            BandRoleTypes[] : array of BandRoleTypeEnum, available Roles : Vocalist, LeadGuitarist, RhythmGuitarist, BassGuitarist, Drummer, KeyboardPlayer

        -  Returns HttpStatus.OK for process done well or HttpStatus.BadRequest for something gone wrong.

  2) CreateUserProfileCommandHandler will handle the command which created in the controller.

  3) If request includes an user which already exists the data will be updated. If not a new user will be created.

### As a user I want to browse (list) pending jams

  1) Request will be handled by the endpoint '{jamApiEndpoint}/api/v1/Jam/{jamStatus}' HTTP.GET Method (Everyone can list jams.)

    - JamStatus can only be Pending, Completed and Started. Pending is already predefined as default

    - Returns the jams with the specified status with HttpStatus.OK, if there is not, returns empty array of Jams.

  2) IJamQueries interface will handle the query of jams and return the jams

### As a user (host) I want to create a public jam that can perform only with specified band roles

  1) Request will be handled by the endpoint '{jamApiEndpoint}/api/v1/Jam/Create' HTTP POST (Only users which are specified as Host in profile creation)

    -Excpected input
        JamType : Enum of ('Private','Public')
        BandRoleTypes[] : array of BandRoleTypeEnum, available Roles : Vocalist, LeadGuitarist, RhythmGuitarist, BassGuitarist, Drummer, KeyboardPlayer

    -  Returns HttpStatus.OK for process done well or HttpStatus.BadRequest for something gone wrong.

  2) CreateJamCommandHandler will handle the command which created in the controller.

  3) The handler creates a jam no matter what. However a jam can have only one different role

### As a user I want to join a publicly available jam if my band role is available in the joining jam

  1) Request will be handled by the endpoint '{jamApiEndpoint}/api/v1/Jam/Register' HTTP PUT (Only users which are specified as Player in profile creation)

    -Excpected input
        JamId : int
        PreferedRole : Enum : Vocalist, LeadGuitarist, RhythmGuitarist, BassGuitarist, Drummer, KeyboardPlayer

    -  Returns HttpStatus.OK for process done well or HttpStatus.BadRequest for something gone wrong.

  2) RegisterToJamCommandHandler will handle the command which created in the controller.

  3) After some controls such as jam does not have the role or there is another user already registered or pending application,
     The handler create this application of a user in pending status of role item if it is okay to create. 

  4) The handler will throw an event UserRegisteredToJamEvent which will be handled by User Api.

  5) This Event handler send a command to validate user role.

  6) If there is an error, UserValidationRegisterJamSuccessEvent will be thrown and handled by Jam Api which updates jam role application status as created and other users can register.

  7) If there is no error, UserValidationRegisterJamFailedEvent will be thrown and handled by Jam Api which updates jam role application status as completed and no other users can register.

  NOTE: There is no notification after this process it can be to-do :)

### As a user (host) I want to start a jam I created when all the roles have assigned performers

  1) Request will be handled by the endpoint '{jamApiEndpoint}/api/v1/Jam/Start' HTTP PUT (Only users which are specified as Host in profile creation)
    -Excpected input
        JamId : int

    -  Returns HttpStatus.OK for process done well or HttpStatus.BadRequest for something gone wrong.

  2) StartJamCommandHandler will handle the command which created in the controller.

  3) After controls such as 'there is a role which is not registered'
     The handler update jam as started status

  4) It will throw an event of JamStartedEvent

  5) This event will be handled by User Api, it will get user emails from its database and will throw an event of SendMailNotificationToUserEvent

### As a user I want to be notified when the jam starts

  1) An email will be sent to user when SendMailNotificationToUserEvent thrown.

  2) There is a background application which handles this event async. (Mail.BackgroundTasks)

  3) This application will send an email to user which is created with email attribute.



## Built With
- NET 6
- Message Resourcing --> RabbitMQ
- MassTransit Framework for messaging
- SAGA Choreography for event handling
- CQRS (Command and Query Responsibility Segregation) 
- MediatR for handling commands.
- Basic Auth with JWT

- Microservices and event driven architecture and DDD 
  APIs: User API, Jam API
  Background Tasks : Mail.BackgroundTasks

- User and Jam are bounded contexts.

- Aggregate
  User Entity is aggregate of Jam

- Builder, UnitOfWork, Repository patterns..

- Docker, docker compose.

- XUnit for creating unit tests.

<!-- GETTING STARTED -->
## Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

### Prerequisites

- Docker to run image

### Run

- Go to project directory
- docker-compose build
  docker-compose up

localhost:5110/swagger/index.html for JamAPI
localhost:5111/swagger/index.html for UserAPI

Note: You might need to restrat Mail.BackgroundTask if there is a connection error between RabbitMQ


<!-- USAGE EXAMPLES -->
## Usage

**
There is an api '{userApiEndpoint}/api/v1/Login/Login' to get bearer access token
first you need to create your user and login with the input of Email and password which are required when you are creating your profile.

There are already players and hosts, please see below;
| Email              | Password  | Type   | Roles                     |
| -----------------  |:---------:| -----: | -----                     |
| berkHost@mail.com  | berkHost  | Host   |                           |
| berkHost2@mail.com | berkHost2 | Host   |                           |
| berkUser@mail.com  | berkUser  | Player | Vocalist                  |
| berkUser2@mail.com | berkUser2 | Player | Vocalist, RhythmGuitarist |
| berkUser3@mail.com | berkUser3 | Player | BassGuitarist             |

**

All the endpoints available in Swagger and you can use it for testing and see the logs go to through docker console.
Everything is logged in applications
