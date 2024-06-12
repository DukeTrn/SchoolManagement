export type IAccount = {
	accountId?: string;
	userName: string;
	fullName: string;
	password?: string;
	createdAt: string;
	modifiedAt?: string;
	isActive?: boolean;
	role: string;
};
