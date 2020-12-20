import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountModel } from 'src/app/models/account.model';
import { AppService } from 'src/app/service/app.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit {

  updateForm;

  constructor(private route: ActivatedRoute,
              private fb: FormBuilder,
              private service: AppService,
              private router: Router) { }

  ngOnInit(): void {
    this.updateForm = this.fb.group({
      userName: [''],
      password: [''],
      id: [0]
    });

    this.route.queryParams.subscribe((result: AccountModel) => {
      // this.updateForm.value.userName = result.userName;
      // this.updateForm.value.password = result.password;
      // this.updateForm.value.id = result.id;

      this.updateForm.reset({userName: result.userName, password: result.password, id: +result.id});
    });
  }

  preventNonNumericalInput(e) {
    var charCode = (typeof e.which == "undefined") ? e.keyCode : e.which;
    var charStr = String.fromCharCode(charCode);

    if (!charStr.match(/^[0-9]+$/))
      e.preventDefault();
  }

  update(){
    console.log(this.updateForm.value);
    if (!this.updateForm.value.password || !this.updateForm.value.userName) return;
    const account = this.updateForm.value as AccountModel;
    account.userName = account.userName.toString();
    this.service.updateAccount(account).subscribe((res) => {
      if (res) {
        this.router.navigate(['detail']);
      } else {
        alert('Account already existed');
      }
    });
  }

}
