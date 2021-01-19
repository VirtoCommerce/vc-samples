const fetch = require("node-fetch");

const getData = async url => {
    try {
        const response = await fetch(url);
        const json = await response.json();
        console.log(json);
        return json;
    }
    catch (error) {
        console.log(error);
    }
};

module.exports = async function (context, req) {

    // Input parameters 
    // base - base (current) item price currency (convert from); default value USD;
    // required - required item currency (convert to); default value EUR;
    // price - item price;
    // rate - price conversation rate (if you need to add some discount after conversation rate should be < 1, 
    // for markup, rate should be > 1, for "as is" conversation use rate = 1); default value 1.

    context.log('JavaScript HTTP trigger function processed a request.');

    let responseBody;
    let responseStatus;

    const baseCurrency = req.query.base ? req.query.base : 'USD';
    const requiredCurrency = req.query.required ? req.query.required : 'EUR';
    const price = parseFloat(req.query.price);
    const rate = req.query.rate ? parseFloat(req.query.rate) : 1;

    const currencyQuery = `https://api.exchangeratesapi.io/latest?symbols=${requiredCurrency}&base=${baseCurrency}`;

    const currencyResponse = await getData(currencyQuery);

    if (currencyResponse && price){

        const currencyRate = currencyResponse.rates[requiredCurrency] ? currencyResponse.rates[requiredCurrency] : 1;
        const convertedPrice = price * currencyRate * rate;

        responseBody = {
            baseCurrency: baseCurrency,
            requiredCurrency: requiredCurrency,
            currencyRate: currencyRate,
            price: price,
            convertedPrice: convertedPrice,
            rate: rate
        }
        responseStatus = 200; // Ok
    }
    else{
        responseStatus = 400; // Bad Request 
        responseBody = '';
        if (!price) responseBody += 'Required input parameter price are not defined \n';
        if (!currencyResponse) responseBody += 'api.exchangeratesapi.io not response \n';
    }

    context.res = {
        status: responseStatus, 
        body: responseBody
    };
}