fibonacci = Subspace()

fibonacci:
    if_le = If(<=)
    sub_left = Operator(-)
    sub_right = Operator(-)
    plus = Operator(+)
    const1 = Const(1)
    output = Output()
    self_left = Subspace(fibonacci)
    self_right = Subspace(fibonacci)

    Input() >> if_le
    Const(2) >> if_le
    if_le.true >> const1
    const1 >> output
    if_le.false >> sub_left
    Const(1) >> sub_left
    sub_left >> self_left
    self_left >> plus
    if_le.false >> sub_right
    Const(2) >> sub_right
    sub_right >> self_right
    self_right >> plus
    plus >> output

Input() >> fibonacci
fibonacci >> Output()
