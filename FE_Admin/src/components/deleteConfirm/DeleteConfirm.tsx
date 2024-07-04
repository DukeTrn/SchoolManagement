import {
	AlertDialog,
	AlertDialogAction,
	AlertDialogCancel,
	AlertDialogContent,
	AlertDialogDescription,
	AlertDialogFooter,
	AlertDialogHeader,
	AlertDialogTitle,
	AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import { Button } from "@/components/ui/button";

interface IProps {
	onClick: any;
	disabled: boolean;
}
const DeleteConfirm = (props: IProps) => {
	const { onClick, disabled } = props;
	return (
		<AlertDialog>
			<AlertDialogTrigger asChild>
				<Button disabled={disabled}>Xóa</Button>
			</AlertDialogTrigger>
			<AlertDialogContent>
				<AlertDialogHeader>
					<AlertDialogTitle>
						Bạn có chắc chắn với hành động này?
					</AlertDialogTitle>
					<AlertDialogDescription>
						Hành động xóa không thể được hoàn tác, nó sẽ xóa vĩnh
						viễn dữ liệu của bạn.
					</AlertDialogDescription>
				</AlertDialogHeader>
				<AlertDialogFooter>
					<AlertDialogCancel>Quay lại</AlertDialogCancel>
					<AlertDialogAction onClick={onClick}>
						Tiếp tục
					</AlertDialogAction>
				</AlertDialogFooter>
			</AlertDialogContent>
		</AlertDialog>
	);
};

export default DeleteConfirm;
