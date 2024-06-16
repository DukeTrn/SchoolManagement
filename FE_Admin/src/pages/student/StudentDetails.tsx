import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import {
	Sheet,
	SheetContent,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";
import { useState } from "react";
import { getStudentDetail } from "@/apis/student.api";
import { IStudent } from "@/types/student.type";

interface IStudentDetailsProps {
	studentId: string;
}

export function StudentDetail(props: IStudentDetailsProps) {
	const { studentId } = props;
	const [student, setStudent] = useState<IStudent>();

	const getStudentDetails = () => {
		getStudentDetail(studentId).then((res) => {
			setStudent(res?.data?.data);
		});
	};

	return (
		<Sheet>
			<SheetTrigger asChild>
				<div
					className="cursor-pointer font-medium text-blue-600 underline"
					onClick={getStudentDetails}
				>
					{studentId}
				</div>
			</SheetTrigger>
			<SheetContent className="max-h-screen overflow-y-scroll">
				<SheetHeader>
					<SheetTitle className="uppercase">
						Chi tiết học sinh
					</SheetTitle>
				</SheetHeader>
				<div className="mt-6">
					<div className="mb-6 flex items-center justify-center">
						<Avatar className="h-28 w-28">
							<AvatarImage src={student?.avatar} alt="@shadcn" />
							<AvatarFallback>AVT</AvatarFallback>
						</Avatar>
					</div>
					<div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Mã số học sinh:
							</span>
							<span>{student?.studentId}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Họ tên học sinh:
							</span>
							<span>{student?.fullName}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Ngày sinh:</span>
							<span>{student?.dob}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Giới tính:</span>
							<span>{student?.gender}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">CCCD:</span>
							<span>{student?.identificationNumber}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Dân tộc:</span>
							<span>{student?.ethnic}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Địa chỉ:</span>
							<span>{student?.address}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Số điện thoại:
							</span>
							<span>{student?.phoneNumber}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Email:</span>
							<span>{student?.email}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Niên khoá:</span>
							<span>{student?.academicYear}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Họ tên bố:</span>
							<span>{student?.fatherName}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Nghề nghiệp bố:
							</span>
							<span>{student?.fatherJob}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">SĐT bố:</span>
							<span>{student?.fatherPhoneNumber}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Họ tên mẹ:</span>
							<span>{student?.motherName}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Nghề nghiệp mẹ:
							</span>
							<span>{student?.motherJob}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">SĐT mẹ:</span>
							<span>{student?.motherPhoneNumber}</span>
						</div>
						<div className="mb-10 flex">
							<span className="mr-4 font-medium">
								Trạng thái học tập:
							</span>
							<span>{student?.status}</span>
						</div>
					</div>
				</div>
			</SheetContent>
		</Sheet>
	);
}
