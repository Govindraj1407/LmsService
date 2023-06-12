# Proxy Classes

This set of classes is to support unit testing since the dynamoDB classes use a bunch of static factories that are difficult to mock.

These classes are excluded from code coverage and absolutely should not contain any logic.  They should only act as a pass thru to the underlying AWS SDK since we cannot test them.