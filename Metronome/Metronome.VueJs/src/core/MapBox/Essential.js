import { getAllStations, getAllIntersections, getTransports } from '../../api/Daemon-Api'

export async function Initialize_MapBox()
{
    let stations = await getAllStations();
    let intersections = await getAllIntersections();
}

export async function Update_MapBox()
{
    let getTrains = await getTransports();
}