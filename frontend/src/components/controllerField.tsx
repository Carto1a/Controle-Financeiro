import { Controller, type Control, type FieldValues, type Path } from "react-hook-form"
import { Field, FieldError, FieldLabel } from "./ui/field"
import { type ReactElement } from "react";

export interface ControllerFieldProps<T extends FieldValues> {
  name: Path<T>,
  label?: string;
  control: Control<T, any, T>
  compoment: (props: { value: any; onChange: (val: any) => void }) => ReactElement;
}

export default function ControllerField<T extends FieldValues>(props: ControllerFieldProps<T>) {
  return (
    <Controller
      name={props.name}
      control={props.control}
      render={control => (
        <Field data-invalid={control.fieldState.invalid} className="gap-1">
          {props.label &&
            (
              <FieldLabel className="font-semibold">
                {props.label}
              </FieldLabel>
            )}
          <props.compoment {...control.field} />
          {control.fieldState.invalid && (
            <FieldError errors={[control.fieldState.error]} />
          )}
        </Field>
      )}
    />
  )
}

