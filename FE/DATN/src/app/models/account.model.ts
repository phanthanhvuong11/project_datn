export class AccountModel {
    id: number;
    userName: string;
    password: string;
    /**
     *
     */
    constructor(init?: Partial<AccountModel>) {
        Object.assign(this, init);
    }
}