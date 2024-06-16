export type IClassroom = {
	classId: string;
	className: string;
	academicYear: string;
	grade: string;
	homeroomTeacherId?: string;
	homeroomTeacherName: string;
};

export type IStudentClassroom = {
	classDetailId: string;
	number: number;
	studentId: string;
	fullName: string;
	phoneNumber: string;
	status: number;
};
