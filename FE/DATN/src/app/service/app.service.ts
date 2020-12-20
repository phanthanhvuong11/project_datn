import { HttpClient } from '@angular/common/http';
import {Injectable} from '@angular/core';
import { Observable, of } from 'rxjs';
import { AccountModel } from '../models/account.model';
import {environment} from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class AppService {
    apiUrl = 'https://localhost:5001';
    constructor(private httpClient: HttpClient) {
    }

    getAccounts(): Observable<AccountModel[]> {
        if (environment.debug) {
            const accounts = [
                new AccountModel({
                    userName: 'Phan Thanh Vuong',
                    password: '123456'
                }),
                new AccountModel({
                    userName: 'Nguyen Van Loc',
                    password: '123456'
                })
            ]
            return of(accounts);
        }
        return this.httpClient.get<AccountModel[]>(`${this.apiUrl}/Account`);
    }

    saveAccounts(account: AccountModel): Observable<boolean> {
        if (environment.debug) {
            return of(true);
        }
        return this.httpClient.post<boolean>(`${this.apiUrl}/Register`, account);
    }

    deleteAccount(id: number): Observable<boolean> {
        if (environment.debug) {
            return of(true);
        }
        return this.httpClient.delete<boolean>(`${this.apiUrl}/Account/${id}`);
    }

    updateAccount(account: AccountModel): Observable<boolean> {
        if (environment.debug) {
            return of(true);
        }
        return this.httpClient.post<boolean>(`${this.apiUrl}/Account/Update`, account);
    }
}
