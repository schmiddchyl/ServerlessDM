console.log('Loading');

const AWS = require('aws-sdk');
const S3 = new AWS.S3();
const puppeteer = require('puppeteer-core');
const chrome = require('chrome-aws-lambda');
const sharp = require('sharp');


exports.handler = async (event) => {
    let result = null;
    let browser = null;

    try {
        browser = await puppeteer.launch({
            args: chrome.args,
            executablePath: await chrome.executablePath,
            headless: chrome.headless,
        });

        const page = await browser.newPage();

        const htmlContent = await getS3Object(event.bucket.name, event.object.key); // Assume getS3Object fetches the HTML content from S3

        await page.setContent(htmlContent);
        const screenshot = await page.screenshot({ format: 'jpeg' });
        const thumbnail = await sharp(screenshot)
            .resize(64, 64) // desired size
            .toBuffer();
        // Now, you can resize the screenshot to a thumbnail using any library.
        // Save the screenshot to S3...

    } finally {
        if (browser !== null) {
            await browser.close();
        }
    }

    return result;
};

async function getS3Object(bucket, key) {
    try {
        const params = {
            Bucket: bucket,
            Key: key
        };

        const data = await S3.getObject(params).promise();
        return data.Body.toString('utf-8'); // Convert the S3 object's body to a string assuming it's utf-8 encoded HTML
    } catch (err) {
        console.error(`Failed to retrieve object from S3. Bucket: ${bucket}, Key: ${key}. Error:`, err);
        throw err; // or handle error appropriately
    }
}