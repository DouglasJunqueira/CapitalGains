# Summary
* The task is to create a command-line program that calculates capital gains tax based on stock trading operations, which are input in JSON format. 
* The program receives a list of transactions (buy or sell) and computes the tax owed on profits from selling stocks, adhering to specific rules. 
* The tax rate is 20% on gains exceeding the weighted average purchase price, and no tax is levied if the total transaction value is below R$ 20,000. 
* The program processes each line independently, maintains no state between lines, and outputs a JSON list of taxes for each transaction. 
* The solution also emphasizes simplicity, elegance, and operational efficiency, with guidelines for error handling and documentation.

## Key Insights
* The program must calculate taxes based on profits from stock sales, following specific tax rules.
* Each transaction is treated independently, requiring careful management of stock quantities and averaged costs.
* The tax is only applied to gains exceeding the average purchase price and if the transaction value exceeds R$ 20,000.
* Losses from previous transactions can offset future gains for tax calculations.
* Clear and concise documentation and code organization are essential for maintainability and future enhancements.
* If a transaction results in a loss, it does not incur any tax and can be used to offset profits from future transactions.
* Transactions with a total value below R$ 20,000 are exempt from taxation, regardless of profit or loss status.

## Observations
* Each line of input is processed independently, meaning the program should not retain any state from previous lines when calculating taxes.

### Input Example
> [{"operation":"buy", "unit-cost":10.00, "quantity": 10000},{"operation":"sell", "unit-cost":20.00, "quantity": 5000}]

### Output Example
> [{"tax":0.00}, {"tax":10000.00}]