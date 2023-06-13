# Technical test automation test ecabs

the following project contains two kinds of tests, which contain the methods for testing the regest api, the automationexercise site and expedia site.


## reqres api test

for the api regest the following tests are contemplated

 1. Successful registration
 ●  POST on https://reqres.in/api/register
 ●  Payload: email and password
 ●  Response: 201 along with a token

 2. Unsuccessful registration
 ● POST on https://reqres.in/api/register
 ● Payload: email
 ● Response: 400 along with an error

 3. Get user list
 ● GET on https://reqres.in/api/users
 ● No payload
 ● Response: 200 with list of users


## Webpage tests https://automationexercise.com/login

The tests contemplated for the site arete

1. Sign in with no credentials. Ensure correct error message is displayed.
2. Sign in with email and empty password. Ensure correct error message is displayed.
3. Sign in with password and empty email. Ensure correct error message is displayed.
4. Sign in with incorrect credentials. Ensure correct error message is displayed.


## Webpage search https://www.expedia.com/


Search for a Multi-City flight with 3 flights (A to B, B to C and C to A). The whole trip should be 1 week long for 4 adults. Ensure that the trip summary (example in Image 1) displays the correct information for all 3 flights Finally select the first trip option (example in Image 2 highlighted in red) and make sure that the trip total (example in Image 3) is equal to price per person (example in Image 2 highlighted in blue) multiplied by the number of people (4).



To run the test you need to have installed.

1.	Visual Studio 2022
2.	Chorme
3.	ChromeDriver


## Usage

1. Open the project with visual studio 2022.
2. Go to the test/test explore tab.
3. Run the test

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.