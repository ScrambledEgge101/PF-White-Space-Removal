# PF-White-Space-Removal
Public Folder Provisioning Script White Space Removal Utility for MigrationWiz

Author: Jason E

Company: BitTitan, Inc.

Date: March 2, 2019

Description: Remove white spaces in the public folder provisioning scripts.

Purpose: When setting up a large public folder migration (>20GB) in MigrationWiz, Powershell scripts need to be provided to the customer so that they can complete their migrations without running into certain problems at the destination. However, the code that generates the Powershell script does not remove white spaces at the beginning and end of the folder names. These white spaces cause problems for the migration. The purpose of this console application is to programmatically remove the white spaces from the public folder provisioning scripts before they are sent to the customers.
