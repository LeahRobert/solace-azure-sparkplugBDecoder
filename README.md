# Azure Function that receives data via HTTP Trigger (eg. from Solace RDP), and decodes DDATA events in SparkplugB protocol to JSON.

## Overview

This project will get you started with receiving HTTP requests in an Azure Function, and decoding SparkplugB protocol (proto2) to JSON for DDATA payload.

## Setting Variables

The Solace hostname and REST port must be specified on line 52 of SpBDecoder.

## Deploy the Azure Function

To publish the Azure Function: https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#publish-to-azure




