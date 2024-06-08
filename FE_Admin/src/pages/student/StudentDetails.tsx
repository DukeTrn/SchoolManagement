import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
	Sheet,
	SheetClose,
	SheetContent,
	SheetDescription,
	SheetFooter,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";

interface IStudentDetailsProps {
	studentId: string;
}

export function StudentDetail(props: IStudentDetailsProps) {
	const { studentId } = props;
	return (
		<Sheet>
			<SheetTrigger asChild>
				<div className="cursor-pointer font-medium text-blue-600 underline">
					{studentId}
				</div>
			</SheetTrigger>
			<SheetContent>
				<SheetHeader>
					<SheetTitle>Chi tiết học sinh</SheetTitle>
				</SheetHeader>
				<div>
					<Label htmlFor="username" className="text-right">
						Email
					</Label>
					<Input id="username" className="w-full" />
				</div>
				<SheetFooter>
					<SheetClose asChild>
						<Button type="submit" className="w-full">
							Lưu
						</Button>
					</SheetClose>
				</SheetFooter>
			</SheetContent>
		</Sheet>
	);
}
