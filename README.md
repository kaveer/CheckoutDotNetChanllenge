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


# configuration
- 
# Future Development



Project name: Your project’s name is the first thing people will see upon scrolling down to your README, and is included upon creation of your README file.

Description: A description of your project follows. A good description is clear, short, and to the point. Describe the importance of your project, and what it does.

Table of Contents: Optionally, include a table of contents in order to allow other people to quickly navigate especially long or detailed READMEs.

Installation: Installation is the next section in an effective README. Tell other users how to install your project locally. Optionally, include a gif to make the process even more clear for other people.

Usage: The next section is usage, in which you instruct other people on how to use your project after they’ve installed it. This would also be a good place to include screenshots of your project in action.

Contributing: Larger projects often have sections on contributing to their project, in which contribution instructions are outlined. Sometimes, this is a separate file. If you have specific contribution preferences, explain them so that other developers know how to best contribute to your work. To learn more about how to help others contribute, check out the guide for setting guidelines for repository contributors.

Credits: Include a section for credits in order to highlight and link to the authors of your project.

License: Finally, include a section for the license of your project. For more information on choosing a license, check out GitHub’s licensing guide!
