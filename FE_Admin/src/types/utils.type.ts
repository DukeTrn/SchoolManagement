export interface IErrorResponseApi<T> {
	message: string;
	data?: T; //generic type
}
export interface ISuccessResponseApi<T> {
	totalCount: number;
	pageNumber: number;
	pageSize: number;
	totalPageCount: number;
	dataList: T;
}

export interface IUpdateResponseApi {
	result: boolean;
	messageType: number;
	message: string;
}

//cu phap -? se loai bo undefine cua key optional (?:)
export type TNoUndefineField<T> = {
	[P in keyof T]-?: TNoUndefineField<NonNullable<T[P]>>;
};
