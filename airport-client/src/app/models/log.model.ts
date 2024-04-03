import Flight from "./flight.model";

export default class Log {

    constructor(public id:number = 0, public flightId:number = 0, public legId:number = 0, public flightStatus:string = "", public passengersCount:number = 0, public inLeg:Date = new Date(), public out:Date = new Date(), public flight:Flight = new Flight()){

    }
}
