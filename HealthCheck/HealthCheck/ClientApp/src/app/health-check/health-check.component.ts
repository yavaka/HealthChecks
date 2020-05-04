import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-health-check',
  templateUrl: './health-check.component.html',
  styleUrls: ['./health-check.component.css']
})

export class HealthCheckComponent
{
  //Property 
  public result: Result; 

  //instantiate HttpClient service and baseUrl variable using DI
  //BASE_URL provider is defined in the main.ts
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  //fetch data with Http Get request to the .Net HealthChecks middleware
  ngOnInit() /*lifecycle hook*/ {
    this.http.get<Result>(this.baseUrl + 'hc')
      .subscribe(result => {
        this.result = result;
      }, error => console.error(error));
  }
}

//JSON request which is recieved from the HealthChecks middleware
interface Result{

  checks: Check[];
  totalStatus: string;
  totalResponseTime: number;
}

//JSON request which is recieved from the HealthChecks middleware
interface Check{

  name: string;
  status: string;
  responseTime: number;
}
