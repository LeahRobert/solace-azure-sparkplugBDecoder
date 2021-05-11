# Azure Function that receives data via HTTP Trigger (eg. from Solace RDP), and decodes DDATA events in SparkplugB protocol to JSON.

## Overview

This project will get you started with receiving HTTP requests in an Azure Function, and decoding SparkplugB protocol to JSON for DDATA payload.

## Prerequisites







####  For C#/.Net using Solace C# APIs

1. Create a new Azure function project in Visual Studio 
    ![ ](img/create-new-vs-project.png)
    Select **Azure Functions** from the list and click **Next**

2. Configure your new project
    ![ ](img/configure-project.png)

3. Create a new Azure Function application
    ![ ](img/create-azure-func-app.png)
   In the above screen you will do the following:
     * Select **Azure Service Bus Trigger** from the list
     * Specifiy **Storage Account**
     * Specify **Connection String Setting Name**. This is the name that we will use in the Azure function code later.
     * Specify Service Bus **Queue Name**. This is the queue that we will stream data from to Solace Event broker.
     * Click **Next** to finish create a project.
  
4. Open the **local.settings.json** file and add the following properties as shown in the code below:

   //Update the Service Bus end point connection string below <br/>
   "SBConnection": "Endpoint=....windows.net/;SharedAccessKeyName=ListenOnly;SharedAccessKey=xxxxxxxxxxxxxxxxxxxxxxx",<br />
   //Update the Solace Host (SMF) URL string below<br/>
   "solace-host": "mr1xi40mbgzuj7.messaging.solace.cloud",<br/>
   //Update the Solace Username string below<br/>
   "solace-username": "solace-cloud-client",<br/>
   //Update the Solace Password string below<br/>
   "solace-password": "abcgdjsjj",<br/>
   //Update the Solace VPN Name string below<br/>
   "solace-vpnname": "sumeet"<br/>
   //Update the Solace Topic string below<br/>
   "solace-topic": "azure/2/solace"<br/>

5. Using NuGet package manager, search and install Solace library.
   ![ ](img/add-solace-library.png)

6. Create a new class called **SolacePublisher** and add the following code to **SolacePublisher.cs** class.


5. Repeat step 8-12 above.
6. Refresh your Solace broker's WebUI to confirm you have received messages from ServiceBus.
      ![ ](img/rest-msg-rcvd.png)