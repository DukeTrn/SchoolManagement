export type IStudent = {
	studentId?: string;
	fullName: string;
	dob: string;
	gender: string;
	phoneNumber: string;
	email: string;
	status: string;
	identificationNumber?: string;
	address: string;
	ethnic: string;
	avatar?: any;
	fatherName: string;
	fatherJob: string;
	fatherPhoneNumber: string;
	fatherEmail?: string;
	motherName: string;
	motherJob: string;
	motherPhoneNumber: string;
	motherEmail?: string;
	academicYear?: string;
};

export type IStudentInfo = Pick<
	IStudent,
	| "studentId"
	| "fullName"
	| "dob"
	| "gender"
	| "phoneNumber"
	| "email"
	| "status"
>;
