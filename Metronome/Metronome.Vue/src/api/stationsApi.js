import {
    getAsync,
    postAsync,
    putAsync,
    deleteAsync
} from "../helpers/apiHelper";

const endpoint = process.env.VUE_APP_BACKEND + "/Transport";

export async function getTimeStopAsync(stopName) {
    return await getAsync(`${endpoint}/GetTrains/${stopName}`);
}
