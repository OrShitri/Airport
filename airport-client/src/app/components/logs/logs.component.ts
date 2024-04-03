import { Component, OnInit } from '@angular/core';
import Log from 'src/app/models/log.model';
import { LogsServiceService } from 'src/app/services/logs-service.service';
import { interval } from 'rxjs';
import Flight from 'src/app/models/flight.model';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.css']
})
export class LogsComponent implements OnInit {

  logs: Log[] = [];
  flights: Flight[] = [];

  constructor(private logService:LogsServiceService){} 

  ngOnInit() {
  
    interval(3000).subscribe(() => {
    this.logService.get()
                    .subscribe(data => {
                      this.logs = data as Log[];
                    })

    this.flights = this.logs
    .filter((log, index, self) => 
        index === self.findIndex((t) => (
            t.flight && t.flight.id === log.flight.id
        ))
    )
    .map(log => ({
        id: log.flight.id,
        flightNumber: log.flight.flightNumber,
        flightStatus: log.flight.flightStatus,
        currentLegId: log.flight.currentLegId,
        brand: log.flight.brand,
        paggengersCount: log.flight.paggengersCount
    }));                
    });  
  }

  getColor(id: number) {
    const colorIndex = id % 10; 
    switch(colorIndex) {
        case 0:
            return 'blue';
        case 1:
            return 'orange'; 
        case 2:
            return 'red';
        case 3:
            return 'green';
        case 4:
            return 'purple';
        case 5:
            return 'yellow';
        case 6:
            return 'cyan'; 
        case 7:
            return 'magenta'; 
        case 8:
            return 'lime'; 
        case 9:
            return 'indigo'; 
        default:
            return 'white'; 
    }
  }

}
