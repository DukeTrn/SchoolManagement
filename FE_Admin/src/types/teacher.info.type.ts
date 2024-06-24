export type ITeacherInfo = {
	teacherId: string;
	fullName: string;
	dob: string;
	gender: string;
	phoneNumber: string;
	email: string;
	level: string;
	status: string;
	departmentId: string;
	departmentName: string;
};

export type ITeacherClass = {
	subjectId: number;
	subjectName: string;
	classId: string;
	className: string;
	academicYear: string;
	grade: number;
	totalStudents: number;
};
