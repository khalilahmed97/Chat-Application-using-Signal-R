import { Component, OnInit } from '@angular/core';
import { ApiServiceService } from '../../services/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit{
  name: string | undefined = "";
  isLoggedIn:boolean = true;

 
  constructor(private apiService:ApiServiceService, private router:Router){
  }
  ngOnInit(): void {
    console.log(this.isLoggedIn)
    this.apiService.status.subscribe({
      next : (res) => {
        if(res==="active"){
          this.isLoggedIn = this.apiService.isLoggedIn()
          console.log(this.isLoggedIn)
          this.name = this.apiService.getUserInfo()?.name 
          this.router.navigate(["/dashboard"])
        }
        else if(res==="inActive"){
          this.isLoggedIn = false
          this.router.navigate([""])
        }
      },
      error: (err) => console.log(err)
    })

  }
}
