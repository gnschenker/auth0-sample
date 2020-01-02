describe("Suite to test the glossary API", () => {
    const uuidv1 = require('uuid/v1');
    const https = require('https');
    const httpsAgent = new https.Agent({ rejectUnauthorized: false });
    const axios = require('axios');
    const host = process.env.API_HOST || "localhost";
    const baseUrl = `http://${host}:5000/api`;
    console.log(`BaseURL: ${baseUrl}`);
    const client = axios.create({
        baseURL: baseUrl,
        httpsAgent
    })
    const utils = require('./utils.js');
    var token = "";
    var headers = "";

    beforeAll(async () => {
        token = await utils.getToken();
        headers = {
            "Authorization": "Bearer " + token,
            "Content-Type": "application/json"
        }
    });
    
    describe("when calling /glossary [GET]", () => {
        it("should return list of glossaries", async () => {
            const response = await client.get('glossary');
            expect(response.status).toBe(200);
            expect(response.data.length).toBeGreaterThan(0);
            expect(response.data[0].term).toBe('AccessToken');
        }) 
    });
    
    describe("when calling /glossary/jwt [GET]", () => {
        it("should return glossary", async () => {
            const response = await client.get('glossary/jwt');
            expect(response.status).toBe(200);
            expect(response.data.term.toLowerCase()).toBe('jwt');
        });
    });

    describe("when callig /glossary/<term> [DELETE] without token", () => {
        it("should fail with error Unauthorized (401)", async () => {
            await expectAsync(client.delete('glossary/jwt'))
                .toBeRejectedWith(new Error("Request failed with status code 401"));
        });
    });

    describe("when callig /glossary [POST] without token", () => {
        it("should fail with error Unauthorized (401)", async () => {
            await expectAsync(client.post('glossary'))
                .toBeRejectedWith(new Error("Request failed with status code 401"));
        });
    });

    describe("when callig /glossary [PUT] without token", () => {
        it("should fail with error Unauthorized (401)", async () => {
            await expectAsync(client.put('glossary'))
                .toBeRejectedWith(new Error("Request failed with status code 401"));
        });
    });

    describe("when creating a new glossary item", () => {
        it("should succeed", async () => {
            const body = {
                term: uuidv1(),
                definition: "An authentication process that considers multiple factors."
            }
            const response = await client.post('glossary', body, { headers: headers});
            
            expect(response.status).toBe(201);
        });
    });

    describe("when creating a new glossary item with duplicate term", () => {
        it("should return a conflict (409) status", async () => {
            const body = {
                term: uuidv1(),
                definition: "Just a sample glossary item."
            }
            const response = await client.post('glossary', body, { headers: headers});

            await expectAsync(client.post('glossary', body, { headers: headers}))
                .toBeRejectedWith(new Error("Request failed with status code 409"));
        })
    })

    describe("when updating an existing glossary item", () => {
        it("should succeed", async () => {
            const body = {
                term: uuidv1(),
                definition: "Just a sample glossary item."
            }
            const response = await client.post('glossary', body, { headers: headers});
            expect(response.status).toBe(201);
            body.description = "Another description...";

            const response2 = await client.put('glossary', body, { headers: headers});

            expect(response2.status).toBe(200);
        })
    })

    describe("when updating a non existing glossary item", () => {
        it("should return a bad request (400) status", async () => {
            const body = {
                term: uuidv1(),
                definition: "Just a sample glossary item."
            }

            await expectAsync(client.put('glossary', body, { headers: headers}))
                .toBeRejectedWith(new Error("Request failed with status code 400"));
        })
    })

    describe("when deleting an existing glossary item", () => {
        it("should return a no content (204) status", async () => {
            const term = uuidv1();
            const body = {
                term: term,
                definition: "Just a sample glossary item."
            }
            const response0 = await client.post('glossary', body, { headers: headers});
            expect(response0.status).toBe(201);
            const uri = 'glossary/'+term;

            const response1 = await client.get(uri);
            expect(response1.status).toBe(200);
            expect(response1.data.term).toBe(term);

            const response = await client.delete(uri, { headers: headers});

            expect(response.status).toBe(204);
        })
    })

    describe("when deleting a non existing glossary item", () => {
        it("should return a not found (404) status", async () => {
            const term = uuidv1();

            await expectAsync(client.delete('glossary/'+term, { headers: headers}))
                .toBeRejectedWith(new Error("Request failed with status code 404"));
        })
    })
});