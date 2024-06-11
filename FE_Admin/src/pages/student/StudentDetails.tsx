import { Button } from "@/components/ui/button";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import {
	Sheet,
	SheetContent,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";
import { useEffect, useState } from "react";

interface IStudentDetailsProps {
	studentId: string;
}

export function StudentDetail(props: IStudentDetailsProps) {
	const { studentId } = props;
	const [data, setData] = useState<any>();

	const getStudentDetails = () => {
		fetch("https://jsonplaceholder.typicode.com/todos/1")
			.then((response) => response.json())
			.then((json) => setData(json));
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
							<AvatarImage
								src="https://github.com/shadcn.png"
								alt="@shadcn"
							/>
							<AvatarFallback>CN</AvatarFallback>
						</Avatar>
					</div>
					<div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Mã số học sinh:
							</span>
							<span>{studentId}</span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Họ tên học sinh:
							</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Ngày sinh:</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Giới tính:</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">CCCD:</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Dân tộc:</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Địa chỉ:</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Số điện thoại:
							</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Email:</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Niên khoá:</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Họ tên bố:</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Nghề nghiệp bố:
							</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">SĐT bố:</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">Họ tên mẹ:</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">
								Nghề nghiệp mẹ:
							</span>
							<span></span>
						</div>
						<div className="mb-3 flex">
							<span className="mr-4 font-medium">SĐT mẹ:</span>
							<span></span>
						</div>
						<div className="mb-10 flex">
							<span className="mr-4 font-medium">
								Trạng thái học tập:
							</span>
							<span></span>
						</div>
					</div>
				</div>
			</SheetContent>
		</Sheet>
	);
}
