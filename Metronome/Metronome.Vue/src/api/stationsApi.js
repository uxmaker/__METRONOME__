import {
    getAsync,
    postAsync,
    putAsync,
    deleteAsync
} from "../helpers/apiHelper";

const endpoint = "http://localhost:5000" + "/Transport";
console.log(endpoint);

export async function getTimeStopAsync(stopId) {
    return await getAsync(`${endpoint}/GetTrains/${stopId}`);
}
