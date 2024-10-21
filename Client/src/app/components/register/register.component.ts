import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ShareModule } from '../../services/share.module';
import { ApiServiceService } from '../../services/api.service';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  isAlert: boolean = false;
  alertType:string = "";
  message: string="";
  registerForm : FormGroup
  constructor(private formBuilder: FormBuilder, private apiService: ApiServiceService){
    this.registerForm = this.formBuilder.group({
      name: formBuilder.control('',[Validators.required]),
      username: formBuilder.control('', [Validators.required]), 
      password: formBuilder.control('', [Validators.required]), 
      confirmPassword: formBuilder.control('', [Validators.required]), 
    })
  }

  register(){
    let loginInformation = {
      name: this.registerForm.get('name')?.value,
      username: this.registerForm.get('username')?.value,
      password: this.registerForm.get('password')?.value,
      confirmPassword: this.registerForm.get('confirmPassword')?.value,
    }
    

    if(loginInformation.password.length < 8){
      this.isAlert=true;
      this.alertType="error"
      this.message="Password must contain 8 digits"
    }

    if(loginInformation.password !== loginInformation.confirmPassword){
      this.isAlert=true;
      this.alertType="error"
      this.message="Password and Confirm Password fields donot match"
    }

    if(loginInformation.password ==null || loginInformation.confirmPassword ==null || loginInformation.name==null || loginInformation.username==null){
      this.isAlert=true;
      this.alertType="error"
      this.message="Please fill all the fields"
    }

    this.apiService.register(loginInformation).subscribe({
      next: (res:any) => {
        let parsedMessage = JSON.parse(res)
        console.log(parsedMessage)
        if(parsedMessage.warning){
          this.isAlert=true;
          this.alertType="warning"
          this.message=parsedMessage.message
        }
        else{
          this.isAlert=true;
          this.alertType="success"
          this.message= res.message
        }
        
      },
      error: (err) => {
       
        this.isAlert=true;
        this.alertType="error"
        this.message=err.message
      }
    })


  }
}
