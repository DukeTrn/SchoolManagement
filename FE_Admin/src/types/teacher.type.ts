export type ITeacher = {
	teacherId: string;
	fullName: string;
	dob: string;
	gender: string;
	phoneNumber: string;
	email: string;
	level: string;
	status: string;
	identificationNumber?: string;
	address: string;
	ethnic: string;
	avatar?: string;
	timeStart: string;
	timeEnd?: string;
	isLeader: boolean;
	isViceLeader: boolean;
};

export type ITeacherInfo = Pick<
	ITeacher,
	| "teacherId"
	| "fullName"
	| "dob"
	| "gender"
	| "phoneNumber"
	| "email"
	| "level"
	| "status"
>;
