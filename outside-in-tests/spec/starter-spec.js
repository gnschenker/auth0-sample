describe("Suite to test the glossary API", () => {
    const https = require('https');
    const httpsAgent = new https.Agent({ rejectUnauthorized: false });
    const axios = require('axios');
    const baseUrl = `https://localhost:5001/api`;
    const client = axios.create({
        baseURL: baseUrl,
        httpsAgent
    })

    describe("when calling /glossary", () => {
        it("should return list of glossaries", async () => {
            const response = await client.get('glossary');
            expect(response.status).toBe(200);
            expect(response.data.length).toBeGreaterThan(0);
            expect(response.data[0].term).toBe('AccessToken');
        }) 
    });
});