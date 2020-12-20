import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountModel } from 'src/app/models/account.model';
import { AppService } from 'src/app/service/app.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm;

  constructor(private fb: FormBuilder,
              private service: AppService,
              private router: Router) { }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });

  }

  preventNonNumericalInput(e) {
    var charCode = (typeof e.which == "undefined") ? e.keyCode : e.which;
    var charStr = String.fromCharCode(charCode);

    if (!charStr.match(/^[0-9]+$/))
      e.preventDefault();
  }

  register() {
    const account = this.registerForm.value as AccountModel;
    account.userName = account.userName.toString();
    this.service.saveAccounts(account).subscribe((res) => {
      console.log(res);
      if (res) {
        this.router.navigate(['detail']);
      } else {
        alert('Account already existed');
      }
    })
  }
}
