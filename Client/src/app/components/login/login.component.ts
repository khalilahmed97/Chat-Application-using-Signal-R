import { Component } from '@angular/core'; 
import { ApiServiceService } from '../../services/api.service';
import { FormBuilder, FormGroup, Validators} from '@angular/forms';
import { ShareModule } from '../../services/share.module';
import { Router } from '@angular/router';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']  // Corrected the key to 'styleUrls'
})
export class LoginComponent {
  isAlert: boolean = false;
  alertType: string = "";
  message: string = "";
  loginForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private apiService: ApiServiceService,private router: Router){
    // Initialize the form with validation rules
    this.loginForm = this.formBuilder.group({
      username: ['khalilsharif2020@outlook.com', [Validators.required]],  // Validators for username
      password: ['Khalil Ahmed Sharif', [Validators.required]],  // Validators for password
    });
  }

  login() {
    // Check if the form is valid before calling the API
    if (this.loginForm.invalid) {
      this.isAlert = true;
      this.alertType = "error";
      this.message = "Please fill all the fields";
      return;
    }

    // Collect user input from the form
    let userInfo = this.loginForm.value;

    // Call the login service
    this.apiService.login(userInfo).subscribe({
      next: (res: any) => {
        let parsedData = JSON.parse(res)
        // No need for JSON.parse since HttpClient returns parsed JSON
        console.log(parsedData);

        if (parsedData.warning) {
          this.isAlert = true;
          this.alertType = "warning";
          this.message = parsedData.message;
        } else {
          this.isAlert = true;
          this.alertType = "success";
          this.message = parsedData.message;
          localStorage.setItem("access_token",parsedData.data);
          this.apiService.status.next("active")
        }
      },
      error: (err) => {
        console.log(err);
        this.isAlert = true;
        this.alertType = "error";
        this.message = "Login failed. Please try again.";
      }
    });
  }
}
