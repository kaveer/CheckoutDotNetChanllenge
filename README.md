# Project Title: CheckoutDotNetChanllenge

# Description
Challenge consist of show case the developer skills in regards to microsoft technologies such as C#, asp.net web API, SQL server. The main challenge consists of developing and simulate a payment gateway transaction between a shopper and the acquiring bank. 

# Architecture
- Acquiring bank
  - Bank (API)
  - Bank.Datalayer
  - Bank.Repository
- Payment gateway
  - PaymentGateway (API)
  - PaymentGateway.Datalayer
  - PaymentGateway.Repository
- Merchant
  - Merchant (Client side)
  
# Installation
```
Install-Package EntityFramework -Version 6.2.0
```
Install EF using the code snippet above on the following projects:
- Bank
- Bank.Datalayer
- Bank.Repository
- PaymentGateway
- PaymentGateway.Datalayer
- PaymentGateway.Repository

```
Install-Package Unity.AspNet.WebApi -Version 5.11.1
```
Instal Unity for dependency injection using the code snippet above on the following projects:
- Bank
- PaymentGateway

```
Install-Package Portable.BouncyCastle -Version 1.8.1.3
```
Install  Bouncy Castle for engryption using the code snippet above in the following projects: 
- Merchant
- Payment Gateway

# Database and ConnectionString
- Database: SQL server
  - Create Database using the script [CreateDatabase.sql](https://github.com/kaveer/CheckoutDotNetChanllenge/blob/master/Database/CreateDatabase.sql)
  - Create table and insert default value in `Bank` database using the script [CreateInsertBankTable.sql](https://github.com/kaveer/CheckoutDotNetChanllenge/blob/master/Database/CreateInsertBankTable.sql)
  - Create table and insert default value in `Merchant` database using the script [CreateInsertMerchantTable.sql](https://github.com/kaveer/CheckoutDotNetChanllenge/blob/master/Database/CreateInsertMerchantTable.sql)
  - Create table and insert default value in `PaymentGateway` database using the script [CreateInsertPaymentGatewayTable.sql](https://github.com/kaveer/CheckoutDotNetChanllenge/blob/master/Database/CreateInsertPaymentGatewayTable.sql)
- ConnectionString
  - Change connection string for bank API in: `~\source\repos\CheckoutDotNetChallenge\Bank\Web.Config`
  - Change connection string for Merchant API in: `~\source\repos\CheckoutDotNetChallenge\Merchant\Site.Config`
  - Change connection string for PaymentGateway API in: `~\source\repos\CheckoutDotNetChallenge\PaymentGateway\Web.Config`
  
# Process : Perform a transaction
## Shopper
1. Shopper will have to enter card details such as: 
    - Card Number
    - Amount
    - Expiry month and year
    - CVC
2. Click on `PAY` button

## Merchant (Client Side)
1. Check if configuration is properly initialize
2. Check if null values which are expected to be input by the user
3. check if the formate input by the user is valid
4. User merchantId to request token from `Payment Gateway` to use as JWT
5. JWT contains merchantId, Valid from/to date, Expiration date
6. Encrypt shopper `card number, CVC, expiration month/ date and amount` using merchant `public key`
7. Add JWT in the header of the http request as authorization 
8. Log transaction

## Payment Gateway
1. Check if there is a token in the incoming request and check if it is valid
2. Extract merchant id from token
3. Decrypt shopper data using merchant `private key` 
4. check if data is valid
5. Get merchant details such as card number, cvc, expiry month/ year from database
6. generate JSON model and make API call to `bank` API using `HttpClient`
7. Log transaction

## Bank
1. check if incoming request body is valid such as card number, amount, cvc...
2. check if expiry month is less than `DateTime.Now.Month`
3. Check if expiry year is less than `DateTime.Now.Year`
4. Check if merchant data is valid
5. Check if card details exit in bank database by matching:
  - Card number
  - CVC
  - Expiry month/ year
6. retrieve shopper card details (Debit account)
7. Check if payment amount is less than shopper balance
8. Remove payment sum from shopper balance
9. Retrieve merchant balance (Credited account)
10. Add payment amount to merchant balance
11. Log transaction
12. Return success status and transaction details as response

# Process: Retrieve previous transaction
### Shopper
1. Click on `View Transaction` Button

### Merchant
1. Redirect to `transaction.aspx`
2. Read and check configuration
3. Request JWT from payment gateway
4. Add JWT as authorization header and request transaction log based on merchant Id to payment gateway
5. Display all transactions and previous transaction

### Payment Gateway
1. check for JWT in incoming request's authorization header
2. extract merchant if from JWT
3. check if token is valid
4. Query database table based on merchant id using Linq statement,
5. Mask card number 
6. Return data to merchant (Client side)

# Endpoint and model
### Bank:
- Add card
  - Endpoint : http://localhost:64624/api/bank/card
  - Http verb : POST
  - Model :
  ```
   public class GatewayPaymentViewModel
    {
        [Required]
        public long CardNumber { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int ExpiryMonth { get; set; }
        [Required]
        public int ExpiryYear { get; set; }
        [Required]
        public int CVC { get; set; }
    }
  ```
- Perform transaction
  - Endpoint : http://localhost:64624/api/bank/sales
  - Http verb: POST
  - Model:
  ```
   public class PaymentResponseViewModel
    {
        public string TransactionId { get; set; }
        public GatewayTransactionDetailsViewModel Details { get; set; }
        public GatewayPaymentViewModel Payment { get; set; }
        public GatewayMerchantViewModel Merchant { get; set; }
    }
  ```
 
### Payment Gateway
- Generate token
  - Endpoint: http://localhost:64591/api/auth/token
  - Http verb: GET
  
- Perform transaction
  - Endpoint: http://localhost:64591/api/transactions/sales
  - Http verb: POST
  - Model:
  ```
   public class OuterMapPaymentViewModel
    {
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string Amount { get; set; }
        [Required]
        public string ExpiryMonth { get; set; }
        [Required]
        public string ExpiryYear { get; set; }
        [Required]
        public string CVC { get; set; }
    }
  ```
- Retrieve transactions
  - Endpoint: http://localhost:64591/api/transactions/retrieve
  - Http verb: GET
  
# Future Development
- This project could have some improvement in the future such as using Azure function or logic apps to simulate the Bank if there was no time constraint and personal commitments. 
- Proper application logging using: serilog
- Develop the client side using single page application technologies such as Angular js or vue js
- Use Asp.net Web API core
- Email notification for success and fail transaction
- Auto generate transaction log for merchante.
- Better encryption mechanism
- Encryption of data between payment gateway and bank
