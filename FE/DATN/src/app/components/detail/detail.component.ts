import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountModel } from 'src/app/models/account.model';
import { AppService } from 'src/app/service/app.service';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss']
})
export class DetailComponent implements OnInit {
  accounts: AccountModel[];
  
  constructor(private service: AppService,
              private router: Router) { }

  ngOnInit(): void {
    this.service.getAccounts().subscribe((accounts) => {
      this.accounts = accounts;
    })
  }

  onDelete(id: number) {
    this.service.deleteAccount(id).subscribe(() => {
      const index = this.accounts.findIndex(_ => _.id === id);
      this.accounts.splice(index, 1);
    });
  }

  onEdit(account: AccountModel) {
    this.router.navigate(['edit'], {queryParams: account});
  }
}
