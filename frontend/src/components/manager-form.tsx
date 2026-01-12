import { Flex } from "@radix-ui/themes";
import { type ReactNode } from "react";
import { FormProvider, type FieldValues, type UseFormReturn } from "react-hook-form";
import { Button } from "./ui/button";
import { Spinner } from "./ui/spinner";

export interface ManagerFormProps<T extends FieldValues> {
  children: ReactNode
  form: UseFormReturn<T, any, T>;
  loading: boolean;
  handleSubmit: (data: T) => void;
  handleCancel: () => void;
}
export default function ManagerForm<T extends FieldValues>(props: ManagerFormProps<T>) {
  return (
    <FormProvider {...props.form}>
      <form onSubmit={props.form.handleSubmit(props.handleSubmit)}>
        <Flex direction="column" gap="3">
          {props.children}
        </Flex>

        <Flex gap="3" mt="4" justify="end">
          <Button
            type="button"
            className="bg-red-500 text-white hover:bg-red-600"
            onClick={props.handleCancel}
          >
            Cancelar
          </Button>
          <Button
            type="submit"
            className="bg-green-500 text-white hover:bg-green-600"
            disabled={props.form.formState.isLoading}
          >
            Criar
            {props.loading && (<Spinner />)}
          </Button>
        </Flex>
      </form>
    </FormProvider>
  )
}
